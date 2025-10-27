import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import {useParams} from "react-router";
import ProductSaleEntryDetail from "./product-sale-entry-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";


const ProductSaleEntry = () => {

    const params = useParams();
    const productSaleId = params.productSaleId;

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        productSaleId: productSaleId,
        nameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
    });

    const [state, setState] = useState({
        productSaleEntryList: [],
        productSaleEntryCount: 0,
        collegeName: "",
        productName: "",
    });

    const loadProductSaleEntryList = () => {
        post("/api/product-sale-entry/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productSaleEntryList: res.data.productSaleEntryList,
                    productSaleEntryCount: res.data.productSaleEntryCount,
                    collegeName: res.data.collegeName,
                    productName: res.data.productName,
                };
            });
        });
    };

    const productSaleEntryDetail = useRef();

    const productSaleEntryListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "版本号",
            dataIndex: "version",
            width: "100px",
        },
        {
            title: "销售时间",
            dataIndex: "saleDate",
            width: "110px",
        },
        {
            title: "服务到期时间",
            dataIndex: "serviceExpireDate",
            width: "150px",
        },
        {
            title: "备注",
            dataIndex: "description",
            width: "300px",
        },
        {
            title: "创建时间",
            dataIndex: "createTime",
            width: "150px",
            sorter: true,
        },
        {
            title: "更新时间",
            dataIndex: "updateTime",
            width: "150px",
            sorter: true,
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const productSaleEntryId = record.id;
                            productSaleEntryDetail.current.show(productSaleEntryId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-sale-entry/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductSaleEntryList();
                                });
                            });
                        }}>
                            <DeleteOutlined/>删除
                        </Button>
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadProductSaleEntryList();
    }, [JSON.stringify(listParam)]);

    const versionQueryRef = useRef();

    return (
        <div>
            <Title title={`销售目录明细 学校:${state.collegeName} 产品:${state.productName}`} menuKey="product-sale-entry" breadcrumb={[
                {title: "销售目录明细"},
            ]}/>
            <div>
                <Space>
                    <Input ref={versionQueryRef} addonBefore="版本号" placeholder="版本号查询" style={{width: 320}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const versionQuery = versionQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    versionQuery: versionQuery,
                                };
                            });
                        }}
                    >
                        查询
                    </Button>
                    <Button
                        color="default"
                        icon={<PlusCircleOutlined style={{color: "green"}}/>}
                        onClick={(e) => {
                            productSaleEntryDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productSaleEntryListColumns} rowKey="id"
                       dataSource={state.productSaleEntryList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.userCount,
                           showQuickJumper: true,
                           showSizeChanger: true,
                           showTotal: (total, range) => `${range[0]}-${range[1]} of ${total}`,
                       }}
                       onChange={(pagination, filters, sorter, extra) => {
                           setListParam((pre) => {
                               return {
                                   ...pre,
                                   currentPage: pagination.current,
                                   pageSize: pagination.pageSize,
                                   sortField: sorter.field,
                                   sortOrder: sorter.order,
                               };
                           });
                       }}
                />
            </div>
            <ProductSaleEntryDetail ref={productSaleEntryDetail} productSaleId={productSaleId} loadProductSaleEntryList={loadProductSaleEntryList}/>
        </div>
    );
};

export default ProductSaleEntry;

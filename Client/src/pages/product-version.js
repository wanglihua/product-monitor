import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import {useParams} from "react-router";
import ProductVersionDetail from "./product-version-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductVersion = () => {

    const params = useParams();
    const productId = params.productId;

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        productId: productId,
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
    });

    const [state, setState] = useState({
        productVersionList: [],
        productVersionCount: 0,
        productName: "",
    });

    const loadProductVersionList = () => {
        post("/api/product-version/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productVersionList: res.data.productVersionList,
                    productVersionCount: res.data.productVersionCount,
                    productName: res.data.productName,
                };
            });
        });
    };

    const productVersionDetail = useRef();

    const productVersionListColumns = [
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
            width: "200px",
        },
        {
            title: "更新基础",
            dataIndex: "versionUpdateBase",
            width: "200px",
        },
        {
            title: "版本更新时间",
            dataIndex: "versionUpdateTime",
            width: "120px",
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
                            const productVersionId = record.id;
                            productVersionDetail.current.show(productVersionId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-version/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductVersionList();
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
        loadProductVersionList();
    }, [JSON.stringify(listParam)]);

    return (
        <div>
            <Title title={`产品版本 产品:${state.productName}`} parentMenuKeys={["basic-setting"]} menuKey="product" breadcrumb={[
                {title: "产品版本"},
            ]}/>
            <div>
                <Space>
                    <Button
                        color="default"
                        icon={<PlusCircleOutlined style={{color: "green"}}/>}
                        onClick={(e) => {
                            productVersionDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productVersionListColumns} rowKey="id"
                       dataSource={state.productVersionList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productVersionCount,
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
            <ProductVersionDetail ref={productVersionDetail} productId={productId} loadProductVersionList={loadProductVersionList}/>
        </div>
    );
};

export default ProductVersion;
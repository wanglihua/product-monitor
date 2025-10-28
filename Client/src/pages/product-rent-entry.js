import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import {useParams, useNavigate} from "react-router";
import ProductRentEntryDetail from "./product-rent-entry-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductRentEntry = () => {

    const params = useParams();
    const productRentId = params.productRentId;

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        productRentId: productRentId,
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
    });

    const [state, setState] = useState({
        productRentEntryList: [],
        productRentEntryCount: 0,
        collegeName: "",
        productName: "",
    });

    const loadProductRentEntryList = () => {
        post("/api/product-rent-entry/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productRentEntryList: res.data.productRentEntryList,
                    productRentEntryCount: res.data.productRentEntryCount,
                    collegeName: res.data.collegeName,
                    productName: res.data.productName,
                };
            });
        });
    };

    const productRentEntryDetail = useRef();

    const productRentEntryListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "累计天数",
            dataIndex: "totalDays",
            width: "110px",
        },
        {
            title: "开始时间",
            dataIndex: "beginDate",
            width: "110px",
        },
        {
            title: "结束时间",
            dataIndex: "endDate",
            width: "110px",
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
                            const productRentEntryId = record.id;
                            productRentEntryDetail.current.show(productRentEntryId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-rent-entry/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);

                                    if (res.data && res.data.productRentDeleted === true) {
                                        navigate("/product-rent");
                                        return;
                                    }
                                    
                                    loadProductRentEntryList();
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
        loadProductRentEntryList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();

    return (
        <div>
            <Title title={`租赁目录明细 学校:${state.collegeName} 产品:${state.productName}`} menuKey="product-rent-entry" breadcrumb={[
                {title: "租赁目录明细"},
            ]}/>
            <div>
                <Space>
                    <Button
                        color="default"
                        icon={<PlusCircleOutlined style={{color: "green"}}/>}
                        onClick={(e) => {
                            productRentEntryDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productRentEntryListColumns} rowKey="id"
                       dataSource={state.productRentEntryList}
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
            <ProductRentEntryDetail ref={productRentEntryDetail} productRentId={productRentId} loadProductRentEntryList={loadProductRentEntryList}/>
        </div>
    );
};

export default ProductRentEntry;

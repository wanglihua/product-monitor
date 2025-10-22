import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import ProductServiceDetail from "./product-service-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductService = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        nameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
    });

    const [state, setState] = useState({
        productServiceList: [],
        productServiceCount: 0,
    });

    const loadProductServiceList = () => {
        post("/api/product-service/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productServiceList: res.data.productServiceList,
                    productServiceCount: res.data.productServiceCount,
                };
            });
        });
    };

    const productServiceDetail = useRef();

    const productServiceListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "学校",
            dataIndex: "collegeId",
            width: "200px",
        },
        {
            title: "产品",
            dataIndex: "productId",
            width: "200px",
        },
        {
            title: "项目类型",
            dataIndex: "projectTypeCode",
            width: "200px",
        },
        {
            title: "售后时间",
            dataIndex: "serviceDate",
            width: "200px",
        },
        {
            title: "售后次数",
            dataIndex: "serviceCount",
            width: "200px",
        },
        {
            title: "售后网址",
            dataIndex: "serviceUrl",
            width: "200px",
        },
        {
            title: "售后账号",
            dataIndex: "loginUser",
            width: "200px",
        },
        {
            title: "售后密码",
            dataIndex: "loginPassword",
            width: "200px",
        },
        {
            title: "联系老师",
            dataIndex: "contactTeacher",
            width: "200px",
        },
        {
            title: "联系电话",
            dataIndex: "contactMobile",
            width: "200px",
        },
        {
            title: "售后问题",
            dataIndex: "description",
            width: "200px",
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
                            const productServiceId = record.id;
                            productServiceDetail.current.show(productServiceId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-service/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductServiceList();
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
        loadProductServiceList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();

    return (
        <div>
            <Title title="售后服务" menuKey="product-service" breadcrumb={[
                {title: "售后服务"},
            ]}/>
            <div>
                <Space>
                    <Input ref={nameQueryRef} addonBefore="名称" placeholder="名称查询" style={{width: 320}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const nameQuery = nameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    nameQuery: nameQuery,
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
                            productServiceDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productServiceListColumns} rowKey="id"
                       dataSource={state.productServiceList}
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
            <ProductServiceDetail ref={productServiceDetail} loadProductServiceList={loadProductServiceList}/>
        </div>
    );
};

export default ProductService;

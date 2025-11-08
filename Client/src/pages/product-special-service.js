import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import ProductSpecialServiceDetail from "./product-special-service-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductSpecialService = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        nameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        productSpecialServiceList: [],
        productSpecialServiceCount: 0,
    });

    const loadProductSpecialServiceList = () => {
        post("/api/product-special-service/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productSpecialServiceList: res.data.productSpecialServiceList,
                    productSpecialServiceCount: res.data.productSpecialServiceCount,
                };
            });
        });
    };

    const productSpecialServiceDetail = useRef();

    const productSpecialServiceListColumns = [
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
            dataIndex: "collegeName",
            width: "180px",
        },
        {
            title: "产品",
            dataIndex: "productName",
            width: "180px",
        },
        {
            title: "联系老师",
            dataIndex: "contactTeacher",
            width: "120px",
        },
        {
            title: "售后次数",
            dataIndex: "serviceCount",
            width: "110px",
        },
        {
            title: "记录时间",
            dataIndex: "updateTime",
            width: "150px",
            sorter: true,
        },
        {
            title: "记录人",
            dataIndex: "userName",
            width: "100px",
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const productSpecialServiceId = record.id;
                            productSpecialServiceDetail.current.show(productSpecialServiceId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-special-service/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductSpecialServiceList();
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
        loadProductSpecialServiceList();
    }, [JSON.stringify(listParam)]);

    const collegeNameQueryRef = useRef();
    const productNameQueryRef = useRef();

    return (
        <div>
            <Title title="特殊售后" menuKey="product-special-service" breadcrumb={[
                {title: "产品服务"},
                {title: "特殊售后"},
            ]}/>
            <div>
                <Space>
                    <Input ref={collegeNameQueryRef} addonBefore="学校名称" placeholder="学校名称查询" style={{width: 280}}/>
                    <Input ref={productNameQueryRef} addonBefore="产品名称" placeholder="产品名称查询" style={{width: 280}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const collegeNameQuery = collegeNameQueryRef.current.input.value;
                            const productNameQuery = productNameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    collegeNameQuery: collegeNameQuery,
                                    productNameQuery: productNameQuery,
                                    current_millis: Date.now(),
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
                            productSpecialServiceDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productSpecialServiceListColumns} rowKey="id"
                       dataSource={state.productSpecialServiceList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productSpecialServiceCount,
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
            <ProductSpecialServiceDetail ref={productSpecialServiceDetail} loadProductSpecialServiceList={loadProductSpecialServiceList}/>
        </div>
    );
};

export default ProductSpecialService;

import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined, UnorderedListOutlined} from '@ant-design/icons';
import {useNavigate} from "react-router";
import ProductDetail from "./product-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const Product = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        nameQuery: "",
        englishNameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
    });

    const [state, setState] = useState({
        productList: [],
        productCount: 0,
    });

    const loadProductList = () => {
        post("/api/product/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productList: res.data.productList,
                    productCount: res.data.productCount,
                };
            });
        });
    };

    const productDetail = useRef();

    const productListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "产品名称",
            dataIndex: "name",
            width: "200px",
        },
        {
            title: "产品英文名",
            dataIndex: "englishName",
            width: "200px",
        },
        {
            title: "版本数量",
            dataIndex: "versionCount",
            width: "120px",
        },
        {
            title: "最新版本号",
            dataIndex: "latestVersion",
            width: "120px",
        },
        {
            title: "更新时间",
            dataIndex: "versionUpdateTime",
            width: "120px",
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const productId = record.id;
                            navigate(`/product-version/${productId}`);
                        }}>
                            <UnorderedListOutlined/>版本列表
                        </Button>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const productId = record.id;
                            productDetail.current.show(productId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductList();
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
        loadProductList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();
    const englishNameQueryRef = useRef();

    return (
        <div>
            <Title title="产品目录" parentMenuKeys={["basic-setting"]} menuKey="product" breadcrumb={[
                {title: "产品目录"},
            ]}/>
            <div>
                <Space>
                    <Input ref={nameQueryRef} addonBefore="产品名称" placeholder="产品名称查询" style={{width: 320}}/>
                    <Input ref={englishNameQueryRef} addonBefore="产品英文名" placeholder="产品英文名查询" style={{width: 320}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const nameQuery = nameQueryRef.current.input.value;
                            const englishNameQuery = englishNameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    nameQuery: nameQuery,
                                    englishNameQuery: englishNameQuery,
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
                            productDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productListColumns} rowKey="id"
                       dataSource={state.productList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productCount,
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
            <ProductDetail ref={productDetail} loadProductList={loadProductList}/>
        </div>
    );
};

export default Product;
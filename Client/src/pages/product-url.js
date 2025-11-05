import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Select, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import ProductUrlDetail from "./product-url-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductUrl = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        typeQuery: 0,
        nameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
    });

    const [state, setState] = useState({
        productUrlList: [],
        productUrlCount: 0,
        productUrlTypeList: [],
    });

    const loadProductUrlList = () => {
        post("/api/product-url/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productUrlList: res.data.productUrlList,
                    productUrlCount: res.data.productUrlCount,
                    productUrlTypeList: res.data.productUrlTypeList,
                };
            });
        });
    };

    const productUrlDetail = useRef();

    const productUrlListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "类型",
            dataIndex: "typeName",
            width: "100px",
        },
        {
            title: "产品名称",
            dataIndex: "name",
            width: "200px",
        },
        {
            title: "网址",
            dataIndex: "url",
            width: "300px",
        },
        // {
        //     title: "账号",
        //     dataIndex: "loginUser",
        //     width: "200px",
        // },
        // {
        //     title: "密码",
        //     dataIndex: "loginPassword",
        //     width: "200px",
        // },
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
                            const productUrlId = record.id;
                            productUrlDetail.current.show(productUrlId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-url/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductUrlList();
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
        loadProductUrlList();
    }, [JSON.stringify(listParam)]);

    const typeQueryRef = useRef();
    const nameQueryRef = useRef();

    return (
        <div>
            <Title title="网址管理" menuKey="product-url" breadcrumb={[
                {title: "网址管理"},
            ]}/>
            <div>
                <Space>
                    <Select options={state.productUrlTypeList} addonbefore="类型" placeholder="类型查询" style={{width: 120}} onSelect={(val) => {
                        typeQueryRef.current = val;
                    }}/>
                    <Input ref={nameQueryRef} addonBefore="产品名称" placeholder="产品名称查询" style={{width: 280}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const nameQuery = nameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    typeQuery: typeQueryRef.current,
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
                            productUrlDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productUrlListColumns} rowKey="id"
                       dataSource={state.productUrlList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productUrlCount,
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
            <ProductUrlDetail ref={productUrlDetail} loadProductUrlList={loadProductUrlList}/>
        </div>
    );
};

export default ProductUrl;

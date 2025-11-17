import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {DeleteOutlined, EditOutlined, FileExcelOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import ProductUpdateDetail from "./product-update-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductUpdate = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        collegeNameQuery: "",
        productNameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        productUpdateList: [],
        productUpdateCount: 0,
    });

    const loadProductUpdateList = () => {
        post("/api/product-update/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productUpdateList: res.data.productUpdateList,
                    productUpdateCount: res.data.productUpdateCount,
                };
            });
        });
    };

    const productUpdateDetail = useRef();

    const productUpdateListColumns = [
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
            width: "200px",
        },
        {
            title: "产品",
            dataIndex: "productName",
            width: "250px",
        },
        {
            title: "销售版本",
            dataIndex: "versionSale",
            width: "120px",
        },
        {
            title: "更新版本",
            dataIndex: "versionUpdate",
            width: "120px",
        },
        {
            title: "更新次数",
            dataIndex: "updateCount",
            width: "110px",
        },
        {
            title: "更新时间",
            dataIndex: "updateDate",
            width: "110px",
        },
        {
            title: "记录人",
            dataIndex: "userName",
            width: "120px",
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const productUpdateId = record.id;
                            productUpdateDetail.current.show(productUpdateId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-update/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductUpdateList();
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
        loadProductUpdateList();
    }, [JSON.stringify(listParam)]);

    const collegeNameQueryRef = useRef();
    const productNameQueryRef = useRef();

    return (
        <div>
            <Title title="产品更新" menuKey="product-update" breadcrumb={[
                {title: "产品服务"},
                {title: "产品更新"},
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
                            productUpdateDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                    <Button
                        color="default"
                        icon={<FileExcelOutlined/>}
                        onClick={(e) => {
                            showConfirm("确认要导出吗！", "确认", () => {
                                const query = $.param(listParam);
                                const url = `/api/product-update/export?${query}`;
                                $(`<form method="post" action="${url}"></form>`).appendTo('body').submit().remove();

                                showSuccess("导出成功, 文件下载中!");
                            });
                        }}
                    >
                        导出
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productUpdateListColumns} rowKey="id"
                       dataSource={state.productUpdateList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productUpdateCount,
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
            <ProductUpdateDetail ref={productUpdateDetail} loadProductUpdateList={loadProductUpdateList}/>
        </div>
    );
};

export default ProductUpdate;

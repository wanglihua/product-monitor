import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import ProductUrlMonitorEntryDetail from "./product-url-monitor-entry-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductUrlMonitorEntry = () => {

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
        productUrlMonitorEntryList: [],
        productUrlMonitorEntryCount: 0,
    });

    const loadProductUrlMonitorEntryList = () => {
        post("/api/product-url-monitor-entry/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productUrlMonitorEntryList: res.data.productUrlMonitorEntryList,
                    productUrlMonitorEntryCount: res.data.productUrlMonitorEntryCount,
                };
            });
        });
    };

    const productUrlMonitorEntryDetail = useRef();

    const productUrlMonitorEntryListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "网站监测",
            dataIndex: "productUrlMonitorId",
            width: "200px",
        },
        {
            title: "账号",
            dataIndex: "loginUser",
            width: "200px",
        },
        {
            title: "密码",
            dataIndex: "loginPassword",
            width: "200px",
        },
        {
            title: "使用地址",
            dataIndex: "ipAddress",
            width: "200px",
        },
        {
            title: "省份",
            dataIndex: "provinceType",
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
                            const productUrlMonitorEntryId = record.id;
                            productUrlMonitorEntryDetail.current.show(productUrlMonitorEntryId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-url-monitor-entry/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductUrlMonitorEntryList();
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
        loadProductUrlMonitorEntryList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();

    return (
        <div>
            <Title title="网站监测详情" menuKey="product-url-monitor-entry" breadcrumb={[
                {title: "网站监测详情"},
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
                            productUrlMonitorEntryDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productUrlMonitorEntryListColumns} rowKey="id"
                       dataSource={state.productUrlMonitorEntryList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productUrlMonitorEntryCount,
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
            <ProductUrlMonitorEntryDetail ref={productUrlMonitorEntryDetail} loadProductUrlMonitorEntryList={loadProductUrlMonitorEntryList}/>
        </div>
    );
};

export default ProductUrlMonitorEntry;

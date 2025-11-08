import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import ProductInstallMonitorEntryDetail from "./product-install-monitor-entry-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductInstallMonitorEntry = () => {

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
        productInstallMonitorEntryList: [],
        productInstallMonitorEntryCount: 0,
    });

    const loadProductInstallMonitorEntryList = () => {
        post("/api/product-install-monitor-entry/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productInstallMonitorEntryList: res.data.productInstallMonitorEntryList,
                    productInstallMonitorEntryCount: res.data.productInstallMonitorEntryCount,
                };
            });
        });
    };

    const productInstallMonitorEntryDetail = useRef();

    const productInstallMonitorEntryListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "安装监测",
            dataIndex: "productMonitorId",
            width: "200px",
        },
        {
            title: "使用账号",
            dataIndex: "loginUser",
            width: "200px",
        },
        {
            title: "账号分类",
            dataIndex: "accountType",
            width: "200px",
        },
        {
            title: "使用IP",
            dataIndex: "ipAddress",
            width: "200px",
        },
        {
            title: "使用省份",
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
                            const productInstallMonitorEntryId = record.id;
                            productInstallMonitorEntryDetail.current.show(productInstallMonitorEntryId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-install-monitor-entry/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductInstallMonitorEntryList();
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
        loadProductInstallMonitorEntryList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();

    return (
        <div>
            <Title title="院校使用详情" menuKey="product-install-monitor-entry" breadcrumb={[
                {title: "院校使用详情"},
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
                            productInstallMonitorEntryDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productInstallMonitorEntryListColumns} rowKey="id"
                       dataSource={state.productInstallMonitorEntryList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productInstallMonitorEntryCount,
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
            <ProductInstallMonitorEntryDetail ref={productInstallMonitorEntryDetail} loadProductInstallMonitorEntryList={loadProductInstallMonitorEntryList}/>
        </div>
    );
};

export default ProductInstallMonitorEntry;

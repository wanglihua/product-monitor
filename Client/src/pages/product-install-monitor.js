import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {SearchOutlined, UnorderedListOutlined} from '@ant-design/icons';
import {useNavigate} from "react-router";
import ProductInstallMonitorDetail from "./product-install-monitor-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductInstallMonitor = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const navigate = useNavigate();

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
        productInstallMonitorList: [],
        productInstallMonitorCount: 0,
    });

    const loadProductInstallMonitorList = () => {
        post("/api/product-install-monitor/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productInstallMonitorList: res.data.productInstallMonitorList,
                    productInstallMonitorCount: res.data.productInstallMonitorCount,
                };
            });
        });
    };

    const productInstallMonitorDetail = useRef();

    const productInstallMonitorListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "使用时间",
            dataIndex: "createTime",
            width: "150px",
            sorter: true,
        },
        {
            title: "学校",
            dataIndex: "collegeName",
            width: "200px",
        },
        {
            title: "产品",
            dataIndex: "productName",
            width: "280px",
        },
        {
            title: "省份",
            dataIndex: "province",
            width: "120px",
        },
        {
            title: "CPU序列号",
            dataIndex: "cpuSerialNo",
            width: "120px",
        },
        {
            title: "网卡硬件地址",
            dataIndex: "netMacAddress",
            width: "130px",
        },
        {
            title: "任务数量",
            align: "center",
            dataIndex: "taskCount",
            width: "90px",
        },
        {
            title: "教师数量",
            align: "center",
            dataIndex: "teacherCount",
            width: "90px",
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const productInstallMonitorId = record.id;
                            navigate(`/product-install-monitor-entry/${productInstallMonitorId}`);
                        }}>
                            <UnorderedListOutlined/>查看详情
                        </Button>
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadProductInstallMonitorList();
    }, [JSON.stringify(listParam)]);

    const collegeNameQueryRef = useRef();
    const productNameQueryRef = useRef();

    return (
        <div>
            <Title title="安装监测" parentMenuKeys={["data-monitor"]} menuKey="product-install-monitor" breadcrumb={[
                {title: "数据监测"},
                {title: "安装监测"},
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
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productInstallMonitorListColumns} rowKey="id"
                       dataSource={state.productInstallMonitorList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productInstallMonitorCount,
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
            <ProductInstallMonitorDetail ref={productInstallMonitorDetail} loadProductInstallMonitorList={loadProductInstallMonitorList}/>
        </div>
    );
};

export default ProductInstallMonitor;

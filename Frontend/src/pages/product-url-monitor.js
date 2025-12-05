import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Select, Space, Table} from 'antd';
import {SearchOutlined, UnorderedListOutlined} from '@ant-design/icons';
import {useNavigate} from "react-router";
import ProductUrlMonitorDetail from "./product-url-monitor-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductUrlMonitor = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        collegeNameQuery: "",
        productNameQuery: "",
        productUrlTypeQuery: 0,
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        productUrlMonitorList: [],
        productUrlMonitorCount: 0,
        productUrlTypeList: [],
    });

    const loadProductUrlMonitorList = () => {
        post("/api/product-url-monitor/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productUrlMonitorList: res.data.productUrlMonitorList,
                    productUrlMonitorCount: res.data.productUrlMonitorCount,
                    productUrlTypeList: res.data.productUrlTypeList,
                };
            });
        });
    };

    const productUrlMonitorDetail = useRef();

    const productUrlMonitorListColumns = [
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
        },
        {
            title: "分类",
            dataIndex: "productUrlType",
            width: "90px",
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
            title: "教师账号数量",
            align: "center",
            dataIndex: "teacherCount",
            width: "120px",
        },
        {
            title: "任务数量",
            align: "center",
            dataIndex: "taskCount",
            width: "100px",
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const productUrlMonitorId = record.id;
                            navigate(`/product-url-monitor-entry/${productUrlMonitorId}`);
                        }}>
                            <UnorderedListOutlined/>查看详情
                        </Button>
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadProductUrlMonitorList();
    }, [JSON.stringify(listParam)]);

    const collegeNameQueryRef = useRef();
    const productNameQueryRef = useRef();
    const productUrlQueryRef = useRef();

    return (
        <div>
            <Title title="网站监测" parentMenuKeys={["data-monitor"]} menuKey="product-url-monitor" breadcrumb={[
                {title: "数据监测"},
                {title: "网站监测"},
            ]}/>
            <div>
                <Space>
                    <Input ref={collegeNameQueryRef} addonBefore="学校名称" placeholder="学校名称查询" style={{width: 280}}/>
                    <Input ref={productNameQueryRef} addonBefore="产品名称" placeholder="产品名称查询" style={{width: 280}}/>
                    <Select options={state.productUrlTypeList} placeholder="分类查询" style={{width: 120}} onSelect={(val) => {
                        productUrlQueryRef.current = val;
                    }}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const collegeNameQuery = collegeNameQueryRef.current.input.value;
                            const productNameQuery = productNameQueryRef.current.input.value;
                            const productUrlTypeQuery = productUrlQueryRef.current;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    collegeNameQuery: collegeNameQuery,
                                    productNameQuery: productNameQuery,
                                    productUrlTypeQuery: productUrlTypeQuery,
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
                <Table columns={productUrlMonitorListColumns} rowKey="id"
                       dataSource={state.productUrlMonitorList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productUrlMonitorCount,
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
            <ProductUrlMonitorDetail ref={productUrlMonitorDetail} loadProductUrlMonitorList={loadProductUrlMonitorList}/>
        </div>
    );
};

export default ProductUrlMonitor;

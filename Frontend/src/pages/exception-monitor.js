import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Select, Space, Table} from 'antd';
import {SearchOutlined} from '@ant-design/icons';
import ExceptionMonitorDetail from "./exception-monitor-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ExceptionMonitor = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        productNameQuery: "",
        provinceTypeQuery: 0,
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        exceptionMonitorList: [],
        exceptionMonitorCount: 0,
        provinceTypeList: [],
    });

    const loadExceptionMonitorList = () => {
        post("/api/exception-monitor/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    exceptionMonitorList: res.data.exceptionMonitorList,
                    exceptionMonitorCount: res.data.exceptionMonitorCount,
                    provinceTypeList: res.data.provinceTypeList,
                };
            });
        });
    };

    const exceptionMonitorDetail = useRef();

    const exceptionMonitorListColumns = [
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
            title: "产品",
            dataIndex: "productName",
            width: "250px",
        },
        {
            title: "使用IP",
            dataIndex: "ipAddress",
            width: "120px",
        },
        {
            title: "使用省份",
            dataIndex: "province",
            width: "150px",
        },
        {
            title: "使用端口",
            dataIndex: "port",
            width: "120px",
        },
        {
            title: "",
        },
    ];

    useEffect(() => {
        loadExceptionMonitorList();
    }, [JSON.stringify(listParam)]);

    const productNameQueryRef = useRef();
    const provinceTypeQueryRef = useRef();

    return (
        <div>
            <Title title="异常检测" parentMenuKeys={["data-monitor"]} menuKey="exception-monitor" breadcrumb={[
                {title: "数据监测"},
                {title: "异常检测"},
            ]}/>
            <div>
                <Space>
                    <Input ref={productNameQueryRef} addonBefore="产品名称" placeholder="产品名称查询" style={{width: 320}}/>
                    <Select
                        showSearch={true}
                        optionFilterProp="label"
                        notFoundContent={"暂无数据, 请输入正确的搜索条件"}
                        options={state.provinceTypeList}
                        placeholder="省份查询"
                        style={{width: 180}}
                        onSelect={(val) => {
                            provinceTypeQueryRef.current = val;
                        }}
                    />
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const provinceTypeQuery = provinceTypeQueryRef.current;
                            const productNameQuery = productNameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    provinceTypeQuery: provinceTypeQuery,
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
                <Table columns={exceptionMonitorListColumns} rowKey="id"
                       dataSource={state.exceptionMonitorList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.exceptionMonitorCount,
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
            <ExceptionMonitorDetail ref={exceptionMonitorDetail} loadExceptionMonitorList={loadExceptionMonitorList}/>
        </div>
    );
};

export default ExceptionMonitor;

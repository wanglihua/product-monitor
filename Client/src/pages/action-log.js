import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table, Tag} from 'antd';
import {SearchOutlined} from '@ant-design/icons';

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ActionLog = () => {
    const {post, showError} = useSiteContext();

    const [listParam, setListParam] = useState({
        loginNameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        actionLogList: [],
        actionLogCount: 0,
    });

    const loadLoginLogList = () => {
        post("/api/action/log/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    actionLogList: res.data.actionLogList,
                    actionLogCount: res.data.actionLogCount,
                };
            });
        });
    };

    const actionLogListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "登录名",
            dataIndex: "loginName",
            width: "100px",
        },
        {
            title: "用户名",
            dataIndex: "userName",
            width: "100px",
        },
        {
            title: "操作类型",
            dataIndex: "type",
            width: "150px"
        },
        {
            title: "操作时间",
            dataIndex: "createTime",
            width: "150px",
        },
        {
            title: "操作描述",
            dataIndex: "actionDesc",
            minWidth: "250px",
        },

    ];

    useEffect(() => {
        loadLoginLogList();
    }, [JSON.stringify(listParam)]);

    const loginNameQueryRef = useRef();

    return (
        <div>
            <Title title="操作日志" parentMenuKeys={["system-manage"]} menuKey="action-log" breadcrumb={[
                {title: "操作日志"},
            ]}/>
            <div>
                <Space>
                    <Input ref={loginNameQueryRef} addonBefore="登录名" placeholder="登录名或用户名查询" style={{width: 380}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const loginNameQuery = loginNameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    loginNameQuery: loginNameQuery,
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
                <Table columns={actionLogListColumns} rowKey="id"
                       tableLayout="auto"
                       dataSource={state.actionLogList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.actionLogCount,
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
                       }}/>
            </div>

        </div>
    );
};

export default ActionLog;
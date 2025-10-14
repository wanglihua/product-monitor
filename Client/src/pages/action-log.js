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
    });

    const [state, setState] = useState({
        loginLogList: [],
        loginLogCount: 0,
    });

    const loadLoginLogList = () => {
        post("/api/admin/user/login/log/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    loginLogList: res.data.loginLogList,
                    loginLogCount: res.data.loginLogCount,
                };
            });
        });
    };

    const loginLogListColumns = [
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
            width: "200px",
        },
        {
            title: "用户名",
            dataIndex: "userName",
            width: "200px",
        },
        {
            title: "登录时间",
            dataIndex: "createTime",
            width: "150px",
        },
        {
            title: "IP",
            dataIndex: "ipAddress",
            width: "150px"
        },
        {
            title: "User Agent",
            dataIndex: "userAgent",
        },
    ];

    useEffect(() => {
        loadLoginLogList();
    }, [JSON.stringify(listParam)]);

    const loginNameQueryRef = useRef();

    return (
        <div>
            <Title title="操作日志" menuKey="action-log" breadcrumb={[
                {title: "操作日志"},
            ]}/>
            <Space direction="vertical" size="large">
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
                                    };
                                });
                            }}
                        >
                            查询
                        </Button>
                    </Space>
                </div>

                <div>
                    <Table columns={loginLogListColumns} rowKey="id"
                           dataSource={state.loginLogList}
                           pagination={{
                               current: listParam.currentPage,
                               pageSize: listParam.pageSize,
                               total: state.loginLogCount,
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
            </Space>
        </div>
    );
};

export default ActionLog;
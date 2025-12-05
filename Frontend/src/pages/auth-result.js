import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {SearchOutlined, SettingOutlined} from '@ant-design/icons';

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const AuthResult = () => {

    const {post, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        nameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "",
        sortOrder: "",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        authResultList: [],
        authResultCount: 0,
    });

    const loadAuthResultList = () => {
        post("/api/auth-result/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    authResultList: res.data.authResultList,
                    authResultCount: res.data.authResultCount,
                };
            });
        });
    };

    const authResultListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "资源类别名称",
            dataIndex: "passResCateName",
            width: "130px",
        },
        {
            title: "资源名称",
            dataIndex: "passResName",
            width: "120px",
        },
        {
            title: "登录名",
            dataIndex: "loginName",
            width: "120px",
        },
        {
            title: "用户名",
            dataIndex: "userName",
            width: "120px",
        },
        {
            title: "群组名称",
            dataIndex: "groupName",
            width: "150px",
        },
        {
            title: "授权时间",
            dataIndex: "createTime",
            width: "150px",
        },
        {
            title: "",
        },
    ];

    useEffect(() => {
        loadAuthResultList();
    }, [JSON.stringify(listParam)]);

    const passResCateNameQueryRef = useRef();
    const passResNameQueryRef = useRef();
    const loginNameQueryRef = useRef();
    const userNameQueryRef = useRef();
    const groupNameQueryRef = useRef();

    return (
        <div>
            <Title title="授权总览" parentMenuKeys={["system-manage"]} menuKey="auth-result" breadcrumb={[
                {title: "系统管理"},
                {title: "授权总览"},
            ]}/>
            <div>
                <Space>
                    <Input ref={passResCateNameQueryRef} addonBefore="资源类别" placeholder="资源类别查询" style={{width: 200}}/>
                    <Input ref={passResNameQueryRef} addonBefore="资源名称" placeholder="资源名称查询" style={{width: 200}}/>
                    <Input ref={loginNameQueryRef} addonBefore="登录名" placeholder="登录名查询" style={{width: 200}}/>
                    <Input ref={userNameQueryRef} addonBefore="用户名" placeholder="用户名查询" style={{width: 200}}/>
                    <Input ref={groupNameQueryRef} addonBefore="群组名称" placeholder="群组名称查询" style={{width: 200}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const passResCateNameQuery = passResCateNameQueryRef.current.input.value;
                            const passResNameQuery = passResNameQueryRef.current.input.value;
                            const loginNameQuery = loginNameQueryRef.current.input.value;
                            const userNameQuery = userNameQueryRef.current.input.value;
                            const groupNameQuery = groupNameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    passResCateNameQuery: passResCateNameQuery,
                                    passResNameQuery: passResNameQuery,
                                    loginNameQuery: loginNameQuery,
                                    userNameQuery: userNameQuery,
                                    groupNameQuery: groupNameQuery,
                                    current_millis: Date.now(),
                                };
                            });
                        }}
                    >
                        查询
                    </Button>
                    <Button
                        color="default"
                        icon={<SettingOutlined/>}
                        onClick={(e) => {
                            post("/api/auth-result/refresh/auth/cache", {}, function (response) {
                                const res = response.data;
                                if (res.success === false) {
                                    showError(res.message);
                                    return;
                                }

                                showSuccess(res.message);
                            });
                        }}
                    >
                        更新授权缓存
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={authResultListColumns} rowKey="id"
                       dataSource={state.authResultList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.authResultCount,
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
        </div>
    );
};

export default AuthResult;

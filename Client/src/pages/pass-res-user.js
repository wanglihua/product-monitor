import React, {useEffect, useRef, useState} from 'react';
import {Badge, Button, Input, Space, Table} from 'antd';
import {DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import {useParams} from "react-router";
import useSiteContext from "../site/site-context";
import Title from "../site/title";

const PassResUser = () => {

    const params = useParams();
    const passResId = params.passResId;

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        passResId: passResId,
        loginNameQuery: "",
        userNameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        passResUserList: [],
        passResUserCount: 0,
        passResName: "",
    });

    const loadPassResUserList = () => {
        post("/api/user-pass-res/user/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    passResUserList: res.data.passResUserList,
                    passResUserCount: res.data.passResUserCount,
                    passResName: res.data.passResName,
                };
            });
        });
    };

    const passResUserListColumns = [
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
            title: "是否授权",
            dataIndex: "grant",
            width: "150px",
            render: (value, record, index) => {
                if (value === true) {
                    return <span><Badge status="success"/>&nbsp;已授权</span>;
                }

                if (value === false) {
                    return <span><Badge status="error"/>&nbsp;未授权</span>;
                }

                return <span></span>;
            }
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        {record.grant === true && (
                            <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                                post("/api/user-pass-res/revoke", {
                                    userId: record.userId,
                                    passResId: passResId,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadPassResUserList();
                                });
                            }}>
                                <DeleteOutlined/>收权
                            </Button>
                        )}

                        {record.grant === false && (
                            <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                                post("/api/user-pass-res/grant", {
                                    userId: record.userId,
                                    passResId: passResId,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadPassResUserList();
                                });
                            }}>
                                <PlusCircleOutlined/>授权
                            </Button>
                        )}
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadPassResUserList();
    }, [JSON.stringify(listParam)]);

    const loginNameQueryRef = useRef();
    const userNameQueryRef = useRef();

    return (
        <div>
            <Title title="用户授权" menuKey="user-pass-res" breadcrumb={[
                {title: "用户授权"},
            ]}/>
            <div>
                <Space>
                    <Input ref={loginNameQueryRef} addonBefore="登录名" placeholder="登录名查询" style={{width: 220}}/>
                    <Input ref={userNameQueryRef} addonBefore="用户名" placeholder="用户名查询" style={{width: 220}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const loginNameQuery = loginNameQueryRef.current.input.value;
                            const userNameQuery = userNameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    loginNameQuery: loginNameQuery,
                                    userNameQuery: userNameQuery,
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
                <Table columns={passResUserListColumns} rowKey="id"
                       dataSource={state.passResUserList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.passResUserCount,
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

export default PassResUser;

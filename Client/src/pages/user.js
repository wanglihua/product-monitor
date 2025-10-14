import React, { useEffect, useRef, useState } from 'react';
import { Button, Input, Space, Table, Tag } from 'antd';
import { DeleteOutlined, EditOutlined, SearchOutlined, UserAddOutlined } from '@ant-design/icons';
import UserDetail from "./user-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const User = () => {
    const { post, showConfirm, showError, showSuccess } = useSiteContext();

    const [listParam, setListParam] = useState({
        loginNameQuery: "",
        userNameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
    });

    const [state, setState] = useState({
        userList: [],
        userCount: 0,
    });

    const loadUserList = () => {
        post("/api/user/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    userList: res.data.userList,
                    userCount: res.data.userCount,
                };
            });
        });
    };

    const userDetail = useRef();

    const userListColumns = [
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
            title: "最近登录",
            dataIndex: "lastLoginTime",
            width: "150px",
            sorter: true,
        },
        {
            title: "创建时间",
            dataIndex: "createTime",
            width: "150px",
            sorter: true,
        },
        {
            title: "操作",
            align: "center",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{ fontSize: "12px" }} onClick={(e) => {
                            const userId = record.id;
                            userDetail.current.show(userId);
                        }}>
                            <EditOutlined />编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{ fontSize: "12px" }} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/user/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadUserList();
                                });
                            });
                        }}>
                            <DeleteOutlined />删除
                        </Button>
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadUserList();
    }, [JSON.stringify(listParam)]);

    const loginNameQueryRef = useRef();
    const userNameQueryRef = useRef();

    return (
        <div>
            <Title title="用户" menuKey="user" breadcrumb={[
                { title: "用户" },
            ]} />

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
                                };
                            });
                        }}
                    >
                        查询
                    </Button>
                    <Button
                        color="default"
                        icon={<UserAddOutlined style={{color: "green"}}/>}
                        onClick={(e) => {
                            userDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={userListColumns} rowKey="id"
                       dataSource={state.userList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.userCount,
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
            <UserDetail ref={userDetail} loadUserList={loadUserList} />
        </div>
    );
}

export default User;
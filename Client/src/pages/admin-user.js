import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {DeleteOutlined, SearchOutlined, UserAddOutlined} from '@ant-design/icons';
import AdminUserDetail from "./admin-user-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const AdminUser = () => {
    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        loginNameQuery: "",
        userNameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "admin.id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        adminList: [],
        adminCount: 0,
    });

    const loadAdminList = () => {
        post("/api/admin/user/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    adminList: res.data.adminList,
                    adminCount: res.data.adminCount,
                };
            });
        });
    };

    const adminUserDetail = useRef();

    const adminListColumns = [
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
            title: "创建时间",
            dataIndex: "createTime",
            width: "150px",
            sorter: true,
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要移除吗！", "确认", () => {
                                post("/api/admin/user/remove", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadAdminList();
                                });
                            });
                        }}>
                            <DeleteOutlined/>移除
                        </Button>
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadAdminList();
    }, [JSON.stringify(listParam)]);

    const loginNameQueryRef = useRef();
    const userNameQueryRef = useRef();

    return (
        <div>
            <Title title="管理员" parentMenuKeys={["system-manage"]} menuKey="admin-user" breadcrumb={[
                {title: "系统管理"},
                {title: "管理员"},
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
                    <Button
                        color="default"
                        icon={<UserAddOutlined style={{color: "green"}}/>}
                        onClick={(e) => {
                            adminUserDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>

            <div style={{padding: "10px 0"}}>
                <Table columns={adminListColumns} rowKey="id"
                       dataSource={state.adminList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.adminCount,
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
            <AdminUserDetail ref={adminUserDetail} loadAdminList={loadAdminList}/>
        </div>
    );
};

export default AdminUser;
import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import {useParams, useNavigate} from "react-router";
import GroupUserDetail from "./group-user-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const GroupUser = () => {

    const params = useParams();
    const groupId = params.groupId;

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        groupId: groupId,
        loginNameQuery: "",
        userNameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        groupUserList: [],
        groupUserCount: 0,
        groupName: "",
    });

    const loadGroupUserList = () => {
        post("/api/group-user/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    groupUserList: res.data.groupUserList,
                    groupUserCount: res.data.groupUserCount,
                    groupName: res.data.groupName,
                };
            });
        });
    };

    const groupUserDetail = useRef();

    const groupUserListColumns = [
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
            width: "120px",
        },
        {
            title: "用户名",
            dataIndex: "userName",
            width: "200px",
        },
        {
            title: "添加时间",
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
                                post("/api/group-user/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadGroupUserList();
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
        loadGroupUserList();
    }, [JSON.stringify(listParam)]);

    const loginNameQueryRef = useRef();
    const userNameQueryRef = useRef();

    return (
        <div>
            <Title title="群组成员" parentMenuKeys={["system-manage"]} menuKey="group" breadcrumb={[
                {title: "系统管理"},
                {
                    title: <span style={{cursor: "pointer"}} onClick={() => {
                        navigate("/group");
                    }}>群组</span>
                },
                {title: "群组成员"},
            ]}/>
            <div>
                <Space>
                    <Input ref={loginNameQueryRef} addonBefore="登录名" placeholder="登录名查询" style={{width: 280}}/>
                    <Input ref={userNameQueryRef} addonBefore="用户名" placeholder="用户名查询" style={{width: 280}}/>
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
                        icon={<PlusCircleOutlined style={{color: "green"}}/>}
                        onClick={(e) => {
                            groupUserDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                    <Button
                        color="default"
                        icon={<DeleteOutlined style={{color: "#ff6b69"}}/>}
                        onClick={(e) => {
                            groupUserDetail.current.show(0);
                        }}
                    >
                        清空
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={groupUserListColumns} rowKey="id"
                       dataSource={state.groupUserList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.groupUserCount,
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
            <GroupUserDetail ref={groupUserDetail} groupId={groupId} loadGroupUserList={loadGroupUserList}/>
        </div>
    );
};

export default GroupUser;

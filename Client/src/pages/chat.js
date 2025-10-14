import React, { useEffect, useRef, useState } from 'react';
import { useNavigate } from "react-router";
import { Button, Input, Space, Table } from 'antd';
import { DeleteOutlined, ProfileOutlined, SearchOutlined } from '@ant-design/icons';

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const AdminChat = () => {
    const navigate = useNavigate();

    const { post, showConfirm, showError, showSuccess } = useSiteContext();

    const [listParam, setListParam] = useState({
        userNameQuery: "",
        titleQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
    });

    const [state, setState] = useState({
        chatList: [],
        chatCount: 0,
    });

    const loadChatList = () => {
        post("/api/admin/chat/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    chatList: res.data.chatList,
                    chatCount: res.data.chatCount,
                };
            });
        });
    };

    const chatListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "用户",
            dataIndex: "userName",
            width: "150px",
            sorter: true,
        },
        {
            title: "问题",
            dataIndex: "title",
            width: "580px",
        },
        {
            title: "问答次数",
            dataIndex: "updateTime",
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
                            const chatId = record.id;
                            navigate(`/admin/chat/message/${chatId}`);
                        }}>
                            <ProfileOutlined />查看
                        </Button>

                        <Button color="danger" variant="text" size="small" style={{ fontSize: "12px" }} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/admin/chat/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadChatList();
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
        loadChatList();
    }, [JSON.stringify(listParam)]);

    const userNameQueryRef = useRef();
    const titleQueryRef = useRef();

    return (
        <div>
            <Title title="对话" menuKey="chat" breadcrumb={[
                { title: "对话" },
            ]} />
            <Space direction="vertical" size="large">
                <div>
                    <Space>
                        <Input ref={userNameQueryRef} addonBefore="用户名" placeholder="用户名查询" style={{ width: 320 }} />
                        <Input ref={titleQueryRef} addonBefore="问题" placeholder="问题查询" style={{ width: 320 }} />
                        <Button
                            color="default"
                            icon={<SearchOutlined />}
                            onClick={(e) => {
                                const userNameQuery = userNameQueryRef.current.input.value;
                                const titleQuery = titleQueryRef.current.input.value;

                                setListParam((pre) => {
                                    return {
                                        ...pre,
                                        userNameQuery: userNameQuery,
                                        titleQuery: titleQuery,
                                    };
                                });
                            }}
                        >
                            查询
                        </Button>
                    </Space>
                </div>

                <div>
                    <Table columns={chatListColumns} rowKey="id"
                        dataSource={state.chatList}
                        pagination={{
                            current: listParam.currentPage,
                            pageSize: listParam.pageSize,
                            total: state.chatCount,
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
            </Space>
        </div>
    );
};

export default AdminChat;
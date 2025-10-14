import React, { useEffect, useState } from 'react';
import { useParams } from "react-router";
import MarkdownView from 'react-showdown';
import { Button, Input, Space } from 'antd';
import { AndroidOutlined, UserOutlined } from '@ant-design/icons';

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const AdminChatMessage = () => {
    const params = useParams();
    const chatId = params.chatId;

    const { post, showError } = useSiteContext();

    const [state, setState] = useState({
        chatId: 0,
        chatTitle: "",
        chatMessageList: [],
        chatMessageCount: 0,
    });

    const loadChatMessageList = () => {
        post("/api/admin/chat/message/list", {
            chatId: chatId,
        }, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    chatId: res.data.chatId,
                    chatTitle: res.data.chatTitle,
                    chatMessageList: res.data.chatMessageList,
                    chatMessageCount: res.data.chatMessageCount,
                };
            });
        });
    };

    useEffect(() => {
        loadChatMessageList();
    }, []);

    return (
        <div>
            <Title title={state.chatTitle} menuKey="chat" breadcrumb={[
                { title: <a href="/admin/chat">对话</a> },
                { title: "对话消息列表" },
                { title: state.chatTitle },
            ]} />
            <Space direction="vertical" size="large">
                {state.chatMessageList.map((chatMessage, index) => (
                    <div key={chatMessage.id}>

                        {chatMessage.role === "user" && (
                            <div className="chat-message">
                                <div className="icon" style={{ color: "blue" }}><UserOutlined /></div>
                                <div className="text">
                                    <div className="time">{chatMessage.createTime}</div>
                                    <div className="message">
                                        <MarkdownView
                                            markdown={chatMessage.message}
                                            options={{ tables: true, emoji: true }}
                                        />
                                    </div>
                                </div>
                            </div>
                        )}

                        {chatMessage.role === "assistant" && (
                            <div className="chat-message">
                                <div className="icon" style={{ color: "red" }}><AndroidOutlined /></div>
                                <div className="text" style={{ border: "1px #f2f2f2 solid" }}>
                                    <div className="time">{chatMessage.createTime}</div>
                                    <div className="message">
                                        <MarkdownView
                                            markdown={chatMessage.message}
                                            options={{ tables: true, emoji: true }}
                                        />
                                    </div>
                                </div>
                            </div>
                        )}
                    </div>
                ))}
            </Space>
        </div>
    );
};

export default AdminChatMessage;
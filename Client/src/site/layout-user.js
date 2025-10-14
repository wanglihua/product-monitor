import axios from "axios";
import { Avatar, Dropdown, Flex, Layout, Space, theme } from "antd";
import React, { useEffect } from 'react';
import { Outlet, useNavigate, useSearchParams } from "react-router";

import useSiteContext from "./site-context";

import {
    PoweroffOutlined,
    UserOutlined
} from '@ant-design/icons';

import "../assets/iconfont/iconfont.css";

const LayoutUser = () => {
    const { token } = theme.useToken();

    const {
        post,
        showError,
        user,
        setUser,
        breadcrumb,
        menuActiveKey,
    } = useSiteContext();

    const navigate = useNavigate();
    const [searchParams] = useSearchParams();

    useEffect(() => {
        const userToken = searchParams.get("token");
        axios.request({
            url: "/api/user/login/check",
            method: 'post',
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Content-Type': 'application/json'
            },
            responseType: 'json',
            responseEncoding: 'utf8',
            data: {
                token: userToken,
            },
            transformRequest: [function (data) {
                return JSON.stringify(data);
            }]
        }).then((response) => {
            const res = response.data;

            setUser({
                userId: res.data.userId,
                loginName: res.data.loginName,
                userName: res.data.userName,
            });

            if (res.data.logined !== true) {
                navigate("/login");
            }
        }).catch((error) => {
            // 这里不显示出错信息
        });
    }, []);

    function getNavMenuItemClassName(menuItemId) {
        if (menuItemId === menuActiveKey) {
            return "user-nav-menu-item user-nav-menu-item-active";
        }

        return "user-nav-menu-item";
    }

    return (
        <Layout>
            <Layout.Header style={{
                padding: '0',
                backgroundColor: "transparent",
                height: "auto",
            }}>
                <div style={{
                    height: "24px",
                    lineHeight: "24px",
                    width: "100%",
                    backgroundColor: "black",
                    color: "gray",
                    fontSize: "12px",
                }}>
                    <span style={{ display: "inline-block", marginLeft: "100px" }}>jingchuangjiaoyu@126.com</span>
                    <span style={{ display: "inline-block", marginLeft: "50px" }}>400-018-6009</span>
                </div>

                <div style={{
                    boxShadow: "0 2px 4px 0 rgba(0, 0, 0, 0.2), 0 3px 10px 0 rgba(0, 0, 0, 0.19)",
                }}>
                    <Flex align="center" justify="space-between" style={{
                        height: "58px",
                        lineHeight: "58px",
                        width: "100%",
                        padding: "0 8px",
                        marginBottom: "0",
                        backgroundImage: "linear-gradient(to bottom, #DB1A1A, #FD5050)",
                    }}>
                        <a href="/" style={{ color: "white", fontSize: "26px", marginLeft: "100px", textDecoration: "none", cursor: "pointer" }}>
                            精创AI智能小助手
                        </a>
                        <span style={{ cursor: 'pointer' }}>
                            <Dropdown placement="bottomRight" menu={{
                                items: [
                                    { key: 'logout', label: (<><PoweroffOutlined />&nbsp;<span>退出登录</span></>) },
                                ],
                                onClick: (e) => {
                                    if (e.key === "logout") {
                                        post("/api/logout", {}, function (response) {
                                            if (response.data.success !== true) {
                                                showError(response.data.message);
                                                return;
                                            }

                                            setUser({
                                                userId: 0,
                                                loginName: "",
                                                userName: "",
                                            });

                                            navigate("/login");
                                        });
                                    }
                                },
                            }}>
                                <Space>
                                    <span style={{ color: 'white' }}>{user.loginName} {user.userName}</span>
                                    <Avatar icon={<UserOutlined />} style={{ marginTop: "-3px" }} />
                                </Space>
                            </Dropdown>
                        </span>
                    </Flex>
                </div>
            </Layout.Header>

            <Layout style={{ marginTop: "8px" }}>
                <Layout.Sider width={120} trigger={null} style={{
                    padding: "20px",
                    margin: "0",
                    backgroundColor: "#DFDFE7",
                    borderRadius: "0",
                    borderRight: "1px solid white",
                }}>
                    <div className="user-nav-menu">

                        <a href="/chat" className={getNavMenuItemClassName("chat")}>
                            <i className="iconfont icon-chat1" />
                            <span>聊天</span>
                        </a>

                        <a href="/tool" className={getNavMenuItemClassName("tool")}>
                            <i className="iconfont icon-tool1" />
                            <span>工具</span>
                        </a>

                    </div>
                </Layout.Sider>
                <Layout>
                    <Layout.Content style={{
                        padding: "0",
                        margin: "0",
                        minHeight: "800px",
                        backgroundColor: "#EBEEF2",
                        borderRadius: "0",
                    }}>
                        <Outlet />
                    </Layout.Content>
                </Layout>
            </Layout>
        </Layout>
    );
};

export default LayoutUser;
import axios from "axios";
import React, { useEffect } from 'react';
import { Avatar, Breadcrumb, Button, Flex, ConfigProvider, Dropdown, Layout, Menu, Space, theme } from "antd";
import { Outlet, useNavigate } from "react-router";

import {
    HomeOutlined,
    MenuFoldOutlined,
    MenuUnfoldOutlined,
    PoweroffOutlined,
    QuestionCircleOutlined,
    SolutionOutlined,
    TeamOutlined,
    UserOutlined,
    WechatOutlined,
} from '@ant-design/icons';

import useSiteContext from "./site-context";
import { isNotEmptyArray } from "../core/empty";

const LayoutAdmin = () => {
    const { token } = theme.useToken();

    const {
        post,
        showError,
        sidebarCollapsed,
        setSidebarCollapsed,
        user,
        setUser,
        breadcrumb,
        menuActiveKey,
    } = useSiteContext();

    const navigate = useNavigate();

    const menuItemStyle = { paddingLeft: "18px" };

    useEffect(() => {
        axios.request({
            url: "/api/admin/login/check",
            method: 'post',
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Content-Type': 'application/json'
            },
            responseType: 'json',
            responseEncoding: 'utf8',
            data: {},
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

    let breadcrumbItems = [
        {
            "title": <><HomeOutlined /><span>Home</span></>,
            onClick: (e) => {
                navigate("/admin")
            },
            className: "home-breadcrum",
        }
    ];

    if (isNotEmptyArray(breadcrumb)) {
        breadcrumbItems = breadcrumbItems.concat(breadcrumb);
    }

    return (
        <Layout>
            <Layout.Header style={{
                padding: '0 5px',
                // backgroundColor: "white",
                display: 'flex',
                alignItems: 'center',
            }}>
                <Flex align="center" justify="space-between" style={{width: "100%"}}>
                    <span style={{ color: "white", fontSize: "22px" }}>
                        <span>AI智能助手</span>
                        <span>后台管理</span>
                    </span>

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
                                <Avatar icon={<UserOutlined />} style={{marginTop: "-3px"}}/>
                            </Space>
                        </Dropdown>
                    </span>
                </Flex>
            </Layout.Header>
            <Layout>
                <Layout.Sider id="layout-sidebar" width={200} trigger={null} collapsible collapsed={sidebarCollapsed} style={{
                    padding: "10px",
                    marginBottom: "10px",
                    backgroundColor: token.colorBgContainer,
                    borderBottomRightRadius: token.borderRadiusLG,
                }}>
                    <ConfigProvider theme={{
                        components: {
                            Menu: {
                                // itemHoverColor: token.colorLogoYellow,
                                // itemSelectedColor: token.colorLogoYellow,
                                // itemBg: '#ececec',
                                // itemSelectedBg: '#ececec',
                                itemBorderRadius: 5,
                            },
                        },
                    }}>
                        <Menu
                            theme="light"
                            mode="inline"
                            selectedKeys={[menuActiveKey]}
                            items={[
                                {
                                    key: "chat",
                                    icon: <WechatOutlined style={{ color: "#1677ff" }} />,
                                    label: "对话",
                                    style: menuItemStyle,
                                    onClick: () => {
                                        navigate("/admin/chat");
                                    }
                                },
                                {
                                    key: "preset-question",
                                    icon: <QuestionCircleOutlined style={{ color: "#1677ff" }} />,
                                    label: "预设问题",
                                    style: menuItemStyle,
                                    onClick: () => {
                                        navigate("/admin/preset/question");
                                    }
                                },
                                {
                                    key: "user",
                                    icon: <UserOutlined style={{ color: "#1677ff" }} />,
                                    label: "用户",
                                    style: menuItemStyle,
                                    onClick: () => {
                                        navigate("/admin/user");
                                    }
                                },
                                {
                                    key: "admin-user",
                                    icon: <TeamOutlined style={{ color: "#1677ff" }} />,
                                    label: "管理员",
                                    style: menuItemStyle,
                                    onClick: () => {
                                        navigate("/admin/admin-user");
                                    }
                                },
                                {
                                    key: "action-log",
                                    icon: <SolutionOutlined style={{ color: "#1677ff" }} />,
                                    label: "操作日志",
                                    style: menuItemStyle,
                                    onClick: () => {
                                        navigate("/action/log");
                                    }
                                },
                            ]}
                        >
                        </Menu>

                    </ConfigProvider>
                </Layout.Sider>
                <Layout>
                    <Flex justify="flex-start">
                        <Button
                            type="text"
                            icon={sidebarCollapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}
                            onClick={() => setSidebarCollapsed(!sidebarCollapsed)}
                            style={{
                                fontSize: '16px',
                                color: 'rgba(0, 0, 0, 0.45)',
                                margin: "4px -8px 0 4px",
                            }}
                        />
                        <Breadcrumb
                            style={{ margin: "10px 10px 0 15px" }}
                            items={breadcrumbItems} />
                    </Flex>
                    <Layout.Content style={{
                        padding: "20px",
                        margin: "10px",
                        minHeight: "800px",
                        backgroundColor: token.colorBgContainer,
                        borderRadius: token.borderRadiusLG,
                    }}>
                        <Outlet />
                    </Layout.Content>
                </Layout>
            </Layout></Layout>
    );
};

export default LayoutAdmin;
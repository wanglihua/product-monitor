import axios from "axios";
import React, {useEffect} from 'react';
import {Avatar, Breadcrumb, Button, Flex, ConfigProvider, Dropdown, Layout, Menu, Space, theme} from "antd";
import {Outlet, useNavigate} from "react-router";

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
import {isNotEmptyArray} from "../core/empty";

const LayoutAdmin = () => {
    const {token} = theme.useToken();

    const {
        post,
        showError,
        sidebarCollapsed,
        setSidebarCollapsed,
        user,
        setUser,
        breadcrumb,
        menuOpenKeys,
        menuActiveKey,
    } = useSiteContext();

    const navigate = useNavigate();

    const menuItemStyle = {paddingLeft: "18px"};

    useEffect(() => {
        axios.request({
            url: "/api/login/check",
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
            "title": <><HomeOutlined/><span>Home</span></>,
            onClick: (e) => {
                navigate("/")
            },
            className: "home-breadcrumb",
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
                    <span style={{color: "white", fontSize: "22px"}}>
                        <span>管理平台</span>
                    </span>

                    <span style={{cursor: 'pointer'}}>
                        <Dropdown placement="bottomRight" menu={{
                            items: [
                                {key: 'logout', label: (<><PoweroffOutlined/>&nbsp;<span>退出登录</span></>)},
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
                                <span style={{color: 'white'}}>{user.loginName} {user.userName}</span>
                                <Avatar icon={<UserOutlined/>} style={{marginTop: "-3px"}}/>
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
                        {/* defaultOpenKeys 不工作, 不知道为啥 */}
                        <Menu
                            theme="light"
                            mode="inline"
                            defaultOpenKeys={menuOpenKeys}
                            selectedKeys={[menuActiveKey]}
                            items={[
                                {
                                    key: "product-service",
                                    icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                    label: "产品服务",
                                    children: [
                                        {
                                            key: "product-install",
                                            icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                            label: "产品安装",
                                            onClick: () => {
                                                navigate("/product-install");
                                            }
                                        },
                                        {
                                            key: "product-update",
                                            icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                            label: "产品更新",
                                            onClick: () => {
                                                navigate("/product-update");
                                            }
                                        },
                                        {
                                            key: "product-sale-service",
                                            icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                            label: "售后维护",
                                            onClick: () => {
                                                navigate("/product-service");
                                            }
                                        },
                                        {
                                            key: "product-special-service",
                                            icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                            label: "特殊售后",
                                            onClick: () => {
                                                navigate("/product-special-service");
                                            }
                                        },
                                    ],
                                },
                                {
                                    key: "project-manage",
                                    icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                    label: "项目管理",
                                    children: [
                                        {
                                            key: "product-sale",
                                            icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                            label: "销售目录",
                                            onClick: () => {
                                                navigate("/product-sale");
                                            }
                                        },
                                        {
                                            key: "product-rent",
                                            icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                            label: "租赁目录",
                                            onClick: () => {
                                                navigate("/product-rent");
                                            }
                                        },
                                        {
                                            key: "product-eval",
                                            icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                            label: "试用目录",
                                            onClick: () => {
                                                navigate("/product-eval");
                                            }
                                        },
                                    ],
                                },
                                {
                                    key: "basic-setting",
                                    icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                    label: "基础设置",
                                    children: [
                                        {
                                            key: "product",
                                            icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                            label: "产品目录",
                                            onClick: () => {
                                                navigate("/product");
                                            }
                                        },
                                        {
                                            key: "college",
                                            icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                            label: "院校目录",
                                            onClick: () => {
                                                navigate("/college");
                                            }
                                        },
                                        {
                                            key: "product-url",
                                            icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                            label: "网址管理",
                                            onClick: () => {
                                                navigate("/product-url");
                                            }
                                        },

                                    ],
                                },
                                {
                                    key: "system-manage",
                                    icon: <SolutionOutlined style={{color: "#1677ff"}}/>,
                                    label: "系统管理",
                                    children: [
                                        {
                                            key: "user",
                                            icon: <UserOutlined style={{color: "#1677ff"}}/>,
                                            label: "用户",
                                            onClick: () => {
                                                navigate("/user");
                                            }
                                        },
                                        {
                                            key: "admin-user",
                                            icon: <TeamOutlined style={{color: "#1677ff"}}/>,
                                            label: "管理员",

                                            onClick: () => {
                                                navigate("/admin/user");
                                            }
                                        },
                                        {
                                            key: "action-log",
                                            icon: <SolutionOutlined style={{color: "#1677ff"}}/>,
                                            label: "操作日志",
                                            onClick: () => {
                                                navigate("/action/log");
                                            }
                                        },
                                    ],
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
                            icon={sidebarCollapsed ? <MenuUnfoldOutlined/> : <MenuFoldOutlined/>}
                            onClick={() => setSidebarCollapsed(!sidebarCollapsed)}
                            style={{
                                fontSize: '16px',
                                color: 'rgba(0, 0, 0, 0.45)',
                                margin: "4px -8px 0 4px",
                            }}
                        />
                        <Breadcrumb
                            style={{margin: "10px 10px 0 15px"}}
                            items={breadcrumbItems}/>
                    </Flex>
                    <Layout.Content style={{
                        padding: "20px",
                        margin: "10px",
                        minHeight: "800px",
                        backgroundColor: token.colorBgContainer,
                        borderRadius: token.borderRadiusLG,
                    }}>
                        <Outlet/>
                    </Layout.Content>
                </Layout>
            </Layout></Layout>
    );
};

export default LayoutAdmin;
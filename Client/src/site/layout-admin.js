import axios from "axios";
import React, {useEffect} from 'react';
import {Avatar, Breadcrumb, Button, Flex, ConfigProvider, Dropdown, Layout, Menu, Space, theme} from "antd";
import {Outlet, useNavigate} from "react-router";

import {
    HomeOutlined,
    MenuFoldOutlined,
    MenuUnfoldOutlined,
    PoweroffOutlined,
    UserOutlined,
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
                            defaultSelectedKeys={menuActiveKey}
                            selectedKeys={[menuActiveKey]}
                            items={[
                                {
                                    key: "data-monitor",
                                    icon: <i className="iconfont icon-data-monitor" style={{color: "#1677ff"}}/>,
                                    label: "数据监测",
                                    children: [
                                        {
                                            key: "product-install-monitor",
                                            icon: <i className="iconfont icon-product-install-monitor" style={{color: "#1677ff"}}/>,
                                            label: "安装监测",
                                            onClick: () => {
                                                navigate("/product-install-monitor");
                                            }
                                        },
                                        {
                                            key: "product-url-monitor",
                                            icon: <i className="iconfont icon-product-url-monitor" style={{color: "#1677ff"}}/>,
                                            label: "网站监测",
                                            onClick: () => {
                                                navigate("/product-url-monitor");
                                            }
                                        },
                                        {
                                            key: "exception-monitor",
                                            icon: <i className="iconfont icon-exception-monitor" style={{color: "#1677ff"}}/>,
                                            label: "异常监测",
                                            onClick: () => {
                                                navigate("/exception-monitor");
                                            }
                                        },
                                    ],
                                },
                                {
                                    key: "product-service",
                                    icon: <i className="iconfont icon-product-service" style={{color: "#1677ff"}}/>,
                                    label: "产品服务",
                                    children: [
                                        {
                                            key: "product-install",
                                            icon: <i className="iconfont icon-product-install" style={{color: "#1677ff"}}/>,
                                            label: "产品安装",
                                            onClick: () => {
                                                navigate("/product-install");
                                            }
                                        },
                                        {
                                            key: "product-update",
                                            icon: <i className="iconfont icon-product-update" style={{color: "#1677ff"}}/>,
                                            label: "产品更新",
                                            onClick: () => {
                                                navigate("/product-update");
                                            }
                                        },
                                        {
                                            key: "product-sale-service",
                                            icon: <i className="iconfont icon-product-sale-service" style={{color: "#1677ff"}}/>,
                                            label: "售后维护",
                                            onClick: () => {
                                                navigate("/product-service");
                                            }
                                        },
                                        {
                                            key: "product-special-service",
                                            icon: <i className="iconfont icon-product-special-service" style={{color: "#1677ff"}}/>,
                                            label: "特殊售后",
                                            onClick: () => {
                                                navigate("/product-special-service");
                                            }
                                        },
                                    ],
                                },
                                {
                                    key: "project-manage",
                                    icon: <i className="iconfont icon-project-manage" style={{color: "#1677ff"}}/>,
                                    label: "项目管理",
                                    children: [
                                        {
                                            key: "product-sale",
                                            icon: <i className="iconfont icon-product-sale" style={{color: "#1677ff"}}/>,
                                            label: "销售目录",
                                            onClick: () => {
                                                navigate("/product-sale");
                                            }
                                        },
                                        {
                                            key: "product-rent",
                                            icon: <i className="iconfont icon-product-rent" style={{color: "#1677ff"}}/>,
                                            label: "租赁目录",
                                            onClick: () => {
                                                navigate("/product-rent");
                                            }
                                        },
                                        {
                                            key: "product-eval",
                                            icon: <i className="iconfont icon-product-eval" style={{color: "#1677ff"}}/>,
                                            label: "试用目录",
                                            onClick: () => {
                                                navigate("/product-eval");
                                            }
                                        },
                                    ],
                                },
                                {
                                    key: "basic-setting",
                                    icon: <i className="iconfont icon-basic-setting" style={{color: "#1677ff"}}/>,
                                    label: "基础设置",
                                    children: [
                                        {
                                            key: "product",
                                            icon: <i className="iconfont icon-product" style={{color: "#1677ff"}}/>,
                                            label: "产品目录",
                                            onClick: () => {
                                                navigate("/product");
                                            }
                                        },
                                        {
                                            key: "college",
                                            icon: <i className="iconfont icon-college" style={{color: "#1677ff"}}/>,
                                            label: "院校目录",
                                            onClick: () => {
                                                navigate("/college");
                                            }
                                        },
                                        {
                                            key: "product-url",
                                            icon: <i className="iconfont icon-product-url" style={{color: "#1677ff"}}/>,
                                            label: "网址管理",
                                            onClick: () => {
                                                navigate("/product-url");
                                            }
                                        },

                                    ],
                                },
                                {
                                    key: "system-manage",
                                    icon: <i className="iconfont icon-system-manage" style={{color: "#1677ff"}}/>,
                                    label: "系统管理",
                                    children: [
                                        {
                                            key: "user",
                                            icon: <i className="iconfont icon-user" style={{color: "#1677ff"}}/>,
                                            label: "用户",
                                            onClick: () => {
                                                navigate("/user");
                                            }
                                        },
                                        {
                                            key: "admin-user",
                                            icon: <i className="iconfont icon-admin-user" style={{color: "#1677ff"}}/>,
                                            label: "管理员",
                                            onClick: () => {
                                                navigate("/admin-user");
                                            }
                                        },
                                        {
                                            key: "group",
                                            icon: <i className="iconfont icon-group" style={{color: "#1677ff"}}/>,
                                            label: "群组",
                                            onClick: () => {
                                                navigate("/group");
                                            }
                                        },
                                        {
                                            key: "pass-res",
                                            icon: <i className="iconfont icon-pass-res" style={{color: "#1677ff"}}/>,
                                            label: "授权资源",
                                            onClick: () => {
                                                navigate("/pass-res");
                                            }
                                        },
                                        {
                                            key: "action-log",
                                            icon: <i className="iconfont icon-action-log" style={{color: "#1677ff"}}/>,
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
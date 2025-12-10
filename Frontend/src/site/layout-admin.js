import axios from "axios";
import React, {useEffect, useState} from 'react';
import {Avatar, Breadcrumb, Button, Flex, ConfigProvider, Dropdown, Layout, Menu, Space, theme} from "antd";
import {Outlet, useNavigate} from "react-router";

import {
    HomeOutlined,
    KeyOutlined,
    LogoutOutlined,
    MenuFoldOutlined,
    MenuUnfoldOutlined,
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

    const [menuItems, setMenuItems] = useState([])

    useEffect(() => {
        post("/api/home/nav/menu/auth/data", {}, function (response) {
            const res = response.data;
            if (res.success !== true) {
                // showError(res.message);
                return;
            }

            const data = res.data;

            let navMenuList = [];

            let dataMonitorChildren = [];

            // 数据监测

            if (data.productInstallMonitor === true) {
                dataMonitorChildren.push({
                    key: "product-install-monitor",
                    icon: <i className="iconfont icon-product-install-monitor" style={{color: "#1677ff"}}/>,
                    label: "安装监测",
                    onClick: () => {
                        navigate("/product-install-monitor");
                    }
                });
            }

            if (data.productUrlMonitor === true) {
                dataMonitorChildren.push({
                    key: "product-url-monitor",
                    icon: <i className="iconfont icon-product-url-monitor" style={{color: "#1677ff"}}/>,
                    label: "网站监测",
                    onClick: () => {
                        navigate("/product-url-monitor");
                    }
                });
            }

            if (data.exceptionMonitor === true) {
                dataMonitorChildren.push({
                    key: "exception-monitor",
                    icon: <i className="iconfont icon-exception-monitor" style={{color: "#1677ff"}}/>,
                    label: "异常监测",
                    onClick: () => {
                        navigate("/exception-monitor");
                    }
                })
            }

            if (data.dataMonitor === true) {
                navMenuList.push({
                    key: "data-monitor",
                    icon: <i className="iconfont icon-data-monitor" style={{color: "#1677ff"}}/>,
                    label: "数据监测",
                    children: dataMonitorChildren
                });
            }

            // 产品服务
            let productServiceChildren = [];

            if (data.productInstall === true) {
                productServiceChildren.push({
                    key: "product-install",
                    icon: <i className="iconfont icon-product-install" style={{color: "#1677ff"}}/>,
                    label: "产品安装",
                    onClick: () => {
                        navigate("/product-install");
                    }
                });
            }

            if (data.productUpdate === true) {
                productServiceChildren.push({
                    key: "product-update",
                    icon: <i className="iconfont icon-product-update" style={{color: "#1677ff"}}/>,
                    label: "产品更新",
                    onClick: () => {
                        navigate("/product-update");
                    }
                });
            }

            if (data.productSaleService === true) {
                productServiceChildren.push({
                    key: "product-sale-service",
                    icon: <i className="iconfont icon-product-sale-service" style={{color: "#1677ff"}}/>,
                    label: "售后维护",
                    onClick: () => {
                        navigate("/product-service");
                    }
                });
            }

            if (data.productSpecialService === true) {
                productServiceChildren.push({
                    key: "product-special-service",
                    icon: <i className="iconfont icon-product-special-service" style={{color: "#1677ff"}}/>,
                    label: "特殊售后",
                    onClick: () => {
                        navigate("/product-special-service");
                    }
                });
            }

            if (data.productServiceAnalyze === true) {
                // 后面补上这个菜单
            }

            if (data.productService === true) {
                navMenuList.push({
                    key: "product-service",
                    icon: <i className="iconfont icon-product-service" style={{color: "#1677ff"}}/>,
                    label: "产品服务",
                    children: productServiceChildren,
                });
            }

            // 项目管理
            let projectManageChildren = [];

            if (data.productSale === true) {
                projectManageChildren.push({
                    key: "product-sale",
                    icon: <i className="iconfont icon-product-sale" style={{color: "#1677ff"}}/>,
                    label: "销售目录",
                    onClick: () => {
                        navigate("/product-sale");
                    }
                });
            }

            if (data.productRent === true) {
                projectManageChildren.push({
                    key: "product-rent",
                    icon: <i className="iconfont icon-product-rent" style={{color: "#1677ff"}}/>,
                    label: "租赁目录",
                    onClick: () => {
                        navigate("/product-rent");
                    }
                });
            }

            if (data.productEval === true) {
                projectManageChildren.push({
                    key: "product-eval",
                    icon: <i className="iconfont icon-product-eval" style={{color: "#1677ff"}}/>,
                    label: "试用目录",
                    onClick: () => {
                        navigate("/product-eval");
                    }
                });
            }

            if (data.projectAnalyze === true) {
                // 后面补上这个菜单
            }

            if (data.projectManage === true) {
                navMenuList.push({
                    key: "project-manage",
                    icon: <i className="iconfont icon-project-manage" style={{color: "#1677ff"}}/>,
                    label: "项目管理",
                    children: projectManageChildren,
                });
            }

            // 基础设置

            let basicSettingChildren = [];

            if (data.product === true) {
                basicSettingChildren.push({
                    key: "product",
                    icon: <i className="iconfont icon-product" style={{color: "#1677ff"}}/>,
                    label: "产品目录",
                    onClick: () => {
                        navigate("/product");
                    }
                });
            }

            if (data.college === true) {
                basicSettingChildren.push({
                    key: "college",
                    icon: <i className="iconfont icon-college" style={{color: "#1677ff"}}/>,
                    label: "院校目录",
                    onClick: () => {
                        navigate("/college");
                    }
                });
            }

            if (data.productUrl === true) {
                basicSettingChildren.push({
                    key: "product-url",
                    icon: <i className="iconfont icon-product-url" style={{color: "#1677ff"}}/>,
                    label: "网址管理",
                    onClick: () => {
                        navigate("/product-url");
                    }
                });
            }

            if (data.basicSetting === true) {
                navMenuList.push({
                    key: "basic-setting",
                    icon: <i className="iconfont icon-basic-setting" style={{color: "#1677ff"}}/>,
                    label: "基础设置",
                    children: basicSettingChildren,
                });
            }

            // 系统管理
            let systemManageChildren = [];

            if (data.user === true) {
                systemManageChildren.push({
                    key: "user",
                    icon: <i className="iconfont icon-user" style={{color: "#1677ff"}}/>,
                    label: "用户",
                    onClick: () => {
                        navigate("/user");
                    }
                });
            }

            if (data.userProvince === true) {
                systemManageChildren.push({
                    key: "userProvince",
                    icon: <i className="iconfont icon-area" style={{color: "#1677ff"}}/>,
                    label: "用户区域",
                    onClick: () => {
                        navigate("/user-province");
                    }
                });
            }

            if (data.group === true) {
                systemManageChildren.push({
                    key: "group",
                    icon: <i className="iconfont icon-group" style={{color: "#1677ff"}}/>,
                    label: "群组",
                    onClick: () => {
                        navigate("/group");
                    }
                });
            }

            if (data.passRes === true) {
                systemManageChildren.push({
                    key: "pass-res",
                    icon: <i className="iconfont icon-pass-res" style={{color: "#1677ff"}}/>,
                    label: "授权资源",
                    onClick: () => {
                        navigate("/pass-res");
                    }
                });
            }

            if (data.passRes === true) {
                systemManageChildren.push({
                    key: "auth-result",
                    icon: <i className="iconfont icon-pass-res-result" style={{color: "#1677ff"}}/>,
                    label: "授权总览",
                    onClick: () => {
                        navigate("/auth-result");
                    }
                });
            }

            if (data.clientKey === true) {
                systemManageChildren.push({
                    key: "client-key",
                    icon: <i className="iconfont icon-client-key" style={{color: "#1677ff"}}/>,
                    label: "客户端密钥",
                    onClick: () => {
                        navigate("/client-key");
                    }
                });
            }

            if (data.admin === true) {
                systemManageChildren.push({
                    key: "admin-user",
                    icon: <i className="iconfont icon-admin-user" style={{color: "#1677ff"}}/>,
                    label: "管理员",
                    onClick: () => {
                        navigate("/admin-user");
                    }
                });
            }

            if (data.actionLog === true) {
                systemManageChildren.push({
                    key: "action-log",
                    icon: <i className="iconfont icon-action-log" style={{color: "#1677ff"}}/>,
                    label: "操作日志",
                    onClick: () => {
                        navigate("/action/log");
                    }
                });
            }

            if (data.systemManage === true) {
                navMenuList.push({
                    key: "system-manage",
                    icon: <i className="iconfont icon-system-manage" style={{color: "#1677ff"}}/>,
                    label: "系统管理",
                    children: systemManageChildren
                });
            }

            setMenuItems(navMenuList);
        });
        // }, [menuOpenKeys, menuActiveKey]);
    }, []);

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
                                {key: 'user-password', label: (<><KeyOutlined/>&nbsp;<span>修改密码</span></>)},
                                {key: 'logout', label: (<><LogoutOutlined/>&nbsp;<span>退出登录</span></>)},
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

                                if (e.key === "user-password") {
                                    navigate("/user-password");
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
                            items={menuItems}
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
import {ExclamationCircleFilled} from "@ant-design/icons";
import {ConfigProvider, Modal, notification, Spin} from "antd";
import zhCn from "antd/locale/zh_CN";
import axios from "axios";
import React, {useState} from "react";
import {BrowserRouter} from "react-router";
import "./app.css";
import "./assets/iconfont/iconfont.css";
import Router from "./router";
import {SiteContext} from "./site/site-context";

const App = () => {
    const [notificationApi, notificationContextHolder] = notification.useNotification();
    const [modal, modalContextHolder] = Modal.useModal();

    // 这个 mask state 只能放在前面了, 因为通用方法里面有用到
    const [mask, setMask] = useState(false);
    const [maskWidth, setMaskWidth] = useState("100%");
    const [maskHeight, setMaskHeight] = useState("100%");

    const showMask = () => {
        setMask(true);
        setMaskWidth(document.body.scrollWidth + "px");
        setMaskHeight(document.body.scrollHeight + "px");
    };

    const hideMask = () => {
        setMask(false);
    };

    // begin 通用方法

    const showSuccess = (message, title, duration) => {
        if (!title) {
            title = "成功";
        }

        if (!duration) {
            duration = 3;
        }

        notificationApi["success"]({
            message: title,
            description: message,
            duration: duration,
        });
    };

    const showWarning = (message, title, onOk) => {
        if (!title) {
            title = "错误";
        }

        modal.warning({
            title: title,
            content: message,
            okText: "确认",
            onOk: () => {
                if (onOk) {
                    onOk();
                }

                // 这个可以自动关闭对话框
                return Promise.resolve();
            },
        });
    };

    const showError = (message, title, onOk) => {
        if (!title) {
            title = "错误";
        }

        modal.error({
            title: title,
            content: message,
            okText: "确认",
            onOk: () => {
                if (onOk) {
                    onOk();
                }

                // 这个可以自动关闭对话框
                return Promise.resolve();
            },
        });
    };

    const showConfirm = (message, title, onOk, extraOptions) => {
        if (!title) {
            title = "确认";
        }

        modal.confirm({
            title: title,
            icon: <ExclamationCircleFilled/>,
            content: message,
            okText: "确认",
            cancelText: "取消",
            onOk: () => {
                if (onOk) {
                    onOk();
                }
            },
            onCancel: () => {
            },
            ...extraOptions,
        });
    };

    const post = (url, data, fun, noMask) => {
        if (noMask !== true) {
            // setMask(true);
            showMask();
        }

        axios.request({
            url: url,
            method: 'post',
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Content-Type': 'application/json'
            },
            responseType: 'json',
            responseEncoding: 'utf8',
            data: data,
            transformRequest: [function (data) {
                return JSON.stringify(data);
            }]
        }).then((response) => {
            // setMask(false);
            hideMask();
            if (fun) {
                fun(response);
            }
        }).catch((error) => {
            // setMask(false);
            hideMask();
            // 往往是URL错误，网络错误等等的
            showError(`处理请求发生错误，请刷新后重试！${error.toString()}`);
        });
    }

    const postForm = (url, data, fun, noMask) => {
        if (noMask !== true) {
            // setMask(true);
            showMask();
        }

        axios.request({
            url: url,
            method: 'post',
            headers: {
                'X-Requested-With': 'XMLHttpRequest',
                'Content-Type': 'application/x-www-form-urlencoded'
            },
            responseType: 'json',
            responseEncoding: 'utf8',
            data: data,
            transformRequest: [function (data) {
                let dataArray = [];
                for (let it in data) {
                    dataArray.push(`${encodeURIComponent(it)}=${encodeURIComponent(data[it])}`);
                }
                return dataArray.join("&")
            }]
        }).then((response) => {
            // setMask(false);
            hideMask();
            if (fun) {
                fun(response);
            }
        }).catch((error) => {
            // setMask(false);
            hideMask();
            // 往往是URL错误，网络错误等等的
            showError(`处理请求发生错误，请刷新后重试或联系客服！${error.toString()}`);
        });
    }

    // end  通用方法

    // begin SiteContext

    const [sidebarCollapsed, setSidebarCollapsed] = useState(false);
    const [breadcrumb, setBreadcrumb] = useState([]);

    const [menuOpenKeys, setMenuOpenKeys] = useState([]);
    const [menuActiveKey, setMenuActiveKey] = useState("");

    const [user, setUser] = useState({userId: 0, loginName: "", userName: ""});

    const siteContextValue = {
        post,
        postForm,
        showSuccess,
        showWarning,
        showError,
        showConfirm,

        showMask,
        hideMask,
        sidebarCollapsed,
        setSidebarCollapsed,
        breadcrumb,
        setBreadcrumb,
        menuOpenKeys,
        setMenuOpenKeys,
        menuActiveKey,
        setMenuActiveKey,
        user,
        setUser,
    };

    // end   SiteContext

    return (
        <ConfigProvider
            locale={zhCn}
            theme={{
                token: {
                    // 这几个是自己定义的名称
                    formMaxWidth: '800px',
                    formButtonMarginTop: '40px',
                    popConfirmDescStyle: {minWidth: "180px", minHeight: "50px"},
                },
                components: {
                    Menu: {},
                },
            }}
        >
            <SiteContext.Provider value={siteContextValue}>
                {notificationContextHolder}
                {modalContextHolder}

                {mask === true && (
                    <div style={{
                        position: "absolute",
                        width: maskWidth,
                        height: maskHeight,
                        backgroundColor: "transparent",
                        // backgroundColor: "rgba(8, 0, 140, 0.3)",
                        // backgroundColor: "#08008c",
                        // opacity: 0.3,
                        zIndex: 999999,
                        cursor: "wait",
                        // display: (mask === true ? "block" : "none"),
                    }}>
                        <div id="site-mask" style={{
                            width: "32px",
                            height: "32px",
                            margin: "0 auto",
                            paddingTop: "15px",
                            opacity: 1,
                        }}>
                            <Spin/>
                        </div>
                    </div>
                )}

                <BrowserRouter basename="">
                    <Router/>
                </BrowserRouter>

            </SiteContext.Provider>
        </ConfigProvider>
    );
}

export default App;

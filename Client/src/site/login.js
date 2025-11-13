import {Alert, Button, Form, Input, Space, theme} from 'antd';
import {useEffect, useRef, useState} from "react";
import {useNavigate} from "react-router";
import {LoginOutlined} from '@ant-design/icons';
import {getTitle} from "../core/config";
import {isNotEmptyStr} from '../core/empty';
import Captcha from './captcha';
import useSiteContext from './site-context';

import {LockOutlined, SafetyOutlined, UserOutlined} from "@ant-design/icons"

const Login = () => {
    document.title = getTitle("登录");

    const navigate = useNavigate();
    const {token} = theme.useToken();
    const {post, showSuccess, setUser, showMask, hideMask} = useSiteContext();

    const [respResult, setRespResult] = useState({
        success: true,
        message: "",
        showCaptcha: false,
    });

    const form = useRef();
    const captcha = useRef();

    useEffect(() => {
        setTimeout(() => {
            if (captcha.current) {
                captcha.current.refresh();
            }
        }, 1000);
    }, []);

    return (
        <div style={{margin: "100px auto", maxWidth: "580px", backgroundColor: "white", padding: "60px 100px"}}>
            <div style={{textAlign: "center"}}>
                <h3 style={{fontSize: "28px"}}>用户登录</h3>
            </div>
            {respResult.success === false && (
                <Alert
                    type="error"
                    showIcon
                    closable
                    message="错误"
                    description={respResult.message}
                    style={{margin: "5px 0 20px 0"}}
                />
            )}

            <Form
                ref={form}
                layout="vertical"
                size='large'
                requiredMark={false}
                onFinish={(values) => {
                    let captchaValue = "";
                    // 也能防止 undefined 的值可能会被提交上去
                    if (isNotEmptyStr(values.captcha) && respResult.showCaptcha === true) {
                        captchaValue = values.captcha;
                    }

                    showMask();
                    post("/api/login", {
                        "loginName": values.loginName,
                        "password": values.password,
                        "captcha": captchaValue,
                    }, function (response) {
                        hideMask();
                        const res = response.data;
                        if (res.success === false) {
                            setRespResult({
                                success: false,
                                message: res.message,
                                showCaptcha: res.showCaptcha,
                            });

                            if (captcha.current) {
                                captcha.current.refresh();
                            }

                            form.current.setFieldsValue({
                                captcha: "",
                            });

                            return;
                        }

                        setUser({
                            userId: res.data.userId,
                            loginName: res.data.loginName,
                            userName: res.data.userName,
                        });

                        showSuccess(res.message);

                        navigate("/");
                    });
                }}
            >
                <Form.Item
                    label="登录名"
                    name="loginName"
                    rules={[
                        {
                            required: true,
                            message: '请输入登录名!',
                        },
                    ]}
                >
                    <Input
                        prefix={<UserOutlined style={{color: "#1677ff"}}/>}
                        maxLength={20}
                        placeholder="登录名"
                        allowClear
                    />
                </Form.Item>
                <Form.Item
                    label="密码"
                    name="password"
                    rules={[
                        {
                            required: true,
                            message: '请输入密码!',
                        },
                    ]}
                >
                    <Input.Password
                        prefix={<LockOutlined style={{color: "#1677ff"}}/>}
                        maxLength={18}
                        placeholder="密码"
                        allowClear
                    />
                </Form.Item>

                {respResult.showCaptcha === true && (
                    <Form.Item
                        label="验证码"
                        name="captcha"
                        extra={
                            <div style={{margin: "10px 0 0 10px"}}>
                                <Space>
                                    <Captcha ref={captcha}/>
                                    &nbsp;
                                    <span
                                        className='link-btn'
                                        onClick={() => {
                                            captcha.current.refresh();
                                        }}
                                    >
                                    看不清, 换一个
                                </span>
                                </Space>
                            </div>
                        }
                        rules={[
                            {
                                required: true,
                                message: '请输入验证码!',
                            },
                        ]}
                    >
                        <Input
                            prefix={<SafetyOutlined style={{color: "#1677ff"}}/>}
                            placeholder="验证码"
                            maxLength={4}
                            allowClear
                        />
                    </Form.Item>
                )}
                <Form.Item style={{marginTop: token.formButtonMarginTop, textAlign: "center"}}>
                    <Space>
                        <Button type="primary" size="large" htmlType="submit" style={{width: "180px"}}>
                            <LoginOutlined />&nbsp;登录
                        </Button>
                    </Space>
                </Form.Item>
            </Form>
        </div>
    );
};

export default Login;
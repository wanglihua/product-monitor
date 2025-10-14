import { Button, ConfigProvider, Descriptions, Form, Input, Modal, Select, Space, theme } from "antd";
import React, { useRef, useState } from "react";
import useSiteContext from "../site/site-context";

const UserDetailInner = (props) => {
    const userId = props.userId;
    const open = props.open;
    const self = props.self;
    const loadUserList = props.loadUserList;

    const { token } = theme.useToken();
    const { post, showSuccess, showError } = useSiteContext();

    let passwordRequired = false;

    const title = useRef("");
    if (userId === 0) {
        title.current = "用户新增";
        passwordRequired = true;
    }

    if (userId > 0) {
        title.current = "用户编辑";
        passwordRequired = false;
    }

    const [state, setState] = useState({
        statusList: [],
    });

    const form = useRef();

    return (
        <Modal
            title={title.current}
            width="680px"
            destroyOnClose={true}
            footer={null}
            open={open}
            onOk={() => {
                if (loadUserList) {
                    loadUserList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadUserList) {
                    loadUserList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/admin/user/detail", {
                        "id": userId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        setState({
                            statusList: data.statusList,
                        });

                        form.current.setFieldsValue({
                            loginName: data.loginName,
                            userName: data.userName,
                            status: data.status,
                        });
                    });
                }
            }}
        >
            <ConfigProvider
                theme={{
                    components: {
                        Form: {
                            itemMarginBottom: 0,
                        },
                    },
                }}
            >

                <Form
                    ref={form}
                    layout="horizontal"
                    size='middle'
                    requiredMark={false}
                    onFinish={(values) => {
                        post("/api/admin/user/save", {
                            id: userId,
                            loginName: values.loginName,
                            userName: values.userName,
                            password: values.password,
                            passwordAgain: values.passwordAgain,
                            status: values.status,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadUserList) {
                                loadUserList();;
                            }

                            self.hide();
                        });
                    }}
                >
                    <Descriptions
                        bordered={true}
                        layout="horizontal"
                        size="small"
                        column={1}
                        labelStyle={{ width: "120px" }}
                    >
                        <Descriptions.Item label="登录名">
                            <Form.Item
                                name="loginName"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入登录名!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={30}
                                    placeholder="登录名"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>

                        <Descriptions.Item label="用户名">
                            <Form.Item
                                name="userName"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入用户名!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={30}
                                    placeholder="用户名"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>

                        <Descriptions.Item label="密码">
                            <Form.Item
                                name="password"
                                rules={[
                                    {
                                        required: passwordRequired,
                                        message: '请输入密码!',
                                    },
                                ]}
                            >
                                <Input.Password
                                    minLength={6}
                                    maxLength={20}
                                    placeholder="密码"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>

                        <Descriptions.Item label="密码重输">
                            <Form.Item
                                name="passwordAgain"
                                rules={[
                                    {
                                        required: passwordRequired,
                                        message: '请输入密码重输!',
                                    },
                                ]}
                            >
                                <Input.Password
                                    minLength={6}
                                    maxLength={20}
                                    placeholder="密码重输"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>

                        <Descriptions.Item label="状态">
                            <Form.Item
                                name="status"
                                rules={[
                                    {
                                        required: true,
                                        message: '请选择用户状态!',
                                    },
                                ]}
                            >
                                <Select options={state.statusList} />
                            </Form.Item>
                        </Descriptions.Item>
                    </Descriptions>

                    <Form.Item style={{ marginTop: token.formButtonMarginTop, textAlign: "right" }}>
                        <Space>
                            <Button type="primary" size="large" htmlType="submit">
                                保存
                            </Button>

                            <Button type="default" size="large" onClick={() => {
                                self.hide();
                            }}>
                                取消
                            </Button>
                        </Space>
                    </Form.Item>
                </Form>

            </ConfigProvider>

        </Modal>
    );
};

class UserDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadUserList = props.loadUserList;

        this.state = {
            userId: -1,
            open: false,
        };
    }

    show = (userId) => {
        this.setState({
            userId: userId,
            open: true,
        });
    }

    hide = () => {
        this.setState({
            userId: -1,
            open: false,
        });
    }

    render() {
        const userId = this.state.userId;
        const open = this.state.open;
        const loadUserList = this.loadUserList;

        return (
            <UserDetailInner
                userId={userId}
                open={open}
                self={this}
                loadUserList={loadUserList}
            />
        )
    }
}

export default UserDetail;
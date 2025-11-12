import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const AuthResultDetailInner = (props) => {
    const authResultId = props.authResultId;
    const open = props.open;
    const self = props.self;
    const loadAuthResultList = props.loadAuthResultList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (authResultId === 0) {
        title.current = "新增";
    }

    if (authResultId > 0) {
        title.current = "编辑";
    }

    const [state, setState] = useState({});

    const form = useRef();

    return (
        <Modal
            title={title.current}
            width="680px"
            destroyOnClose={true}
            footer={null}
            open={open}
            onOk={() => {
                if (loadAuthResultList) {
                    loadAuthResultList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadAuthResultList) {
                    loadAuthResultList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/auth-result/detail", {
                        "id": authResultId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            passResCateId: data.passResCateId,
                            passResCateName: data.passResCateName,
                            passResId: data.passResId,
                            passResName: data.passResName,
                            userId: data.userId,
                            loginName: data.loginName,
                            userName: data.userName,
                            groupId: data.groupId,
                            groupName: data.groupName,
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
                        post("/api/auth-result/save", {
                            id: authResultId,
                            passResCateId: values.passResCateId,
                            passResCateName: values.passResCateName,
                            passResId: values.passResId,
                            passResName: values.passResName,
                            userId: values.userId,
                            loginName: values.loginName,
                            userName: values.userName,
                            groupId: values.groupId,
                            groupName: values.groupName,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadAuthResultList) {
                                loadAuthResultList();
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
                        labelStyle={{width: "120px"}}
                    >
                        <Descriptions.Item label="">
                            <Form.Item
                                name="passResCateId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder=""
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item
                                name="passResCateName"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder=""
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item
                                name="passResId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder=""
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item
                                name="passResName"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder=""
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item
                                name="userId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder=""
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item
                                name="loginName"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder=""
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item
                                name="userName"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder=""
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item
                                name="groupId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder=""
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item
                                name="groupName"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder=""
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                    </Descriptions>

                    <Form.Item style={{marginTop: token.formButtonMarginTop, textAlign: "right"}}>
                        <Space>
                            <Button type="primary" size="large" htmlType="submit">
                                <SaveOutlined/>保存
                            </Button>

                            <Button type="default" size="large" onClick={() => {
                                self.hide();
                            }}>
                                <CloseCircleOutlined/>取消
                            </Button>
                        </Space>
                    </Form.Item>
                </Form>

            </ConfigProvider>
        </Modal>
    );
};

class AuthResultDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadAuthResultList = props.loadAuthResultList;

        this.state = {
            authResultId: 0,
            open: false,
        };
    }

    show = (authResultId) => {
        this.setState((pre) => {
           return {
                ...pre,
                authResultId: authResultId,
                open: true,
           };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                authResultId: 0,
                open: false,
            };
        });
    }

    render() {
        const authResultId = this.state.authResultId;
        const open = this.state.open;
        const loadAuthResultList = this.loadAuthResultList;

        return (
            <AuthResultDetailInner
                authResultId={authResultId}
                open={open}
                self={this}
                loadAuthResultList={loadAuthResultList}
            />
        )
    }
}

export default AuthResultDetail;

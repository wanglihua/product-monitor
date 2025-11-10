import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const UserPassResDetailInner = (props) => {
    const userPassResId = props.userPassResId;
    const open = props.open;
    const self = props.self;
    const loadUserPassResList = props.loadUserPassResList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (userPassResId === 0) {
        title.current = "用户授权新增";
    }

    if (userPassResId > 0) {
        title.current = "用户授权编辑";
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
                if (loadUserPassResList) {
                    loadUserPassResList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadUserPassResList) {
                    loadUserPassResList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/user-pass-res/detail", {
                        "id": userPassResId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            userId: data.userId,
                            passResId: data.passResId,
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
                        post("/api/user-pass-res/save", {
                            id: userPassResId,
                            userId: values.userId,
                            passResId: values.passResId,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadUserPassResList) {
                                loadUserPassResList();
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
                        <Descriptions.Item label="用户">
                            <Form.Item
                                name="userId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入用户!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="用户"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="授权资源">
                            <Form.Item
                                name="passResId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入授权资源!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="授权资源"
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

class UserPassResDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadUserPassResList = props.loadUserPassResList;

        this.state = {
            userPassResId: 0,
            open: false,
        };
    }

    show = (userPassResId) => {
        this.setState((pre) => {
           return {
                ...pre,
                userPassResId: userPassResId,
                open: true,
           };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                userPassResId: 0,
                open: false,
            };
        });
    }

    render() {
        const userPassResId = this.state.userPassResId;
        const open = this.state.open;
        const loadUserPassResList = this.loadUserPassResList;

        return (
            <UserPassResDetailInner
                userPassResId={userPassResId}
                open={open}
                self={this}
                loadUserPassResList={loadUserPassResList}
            />
        )
    }
}

export default UserPassResDetail;

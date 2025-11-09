import {Button, ConfigProvider, Descriptions, Form, Input, Modal, Space, theme} from "antd";
import React, {useRef} from "react";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";
import useSiteContext from "../site/site-context";

const AdminUserDetailInner = (props) => {
    const adminId = props.adminId;
    const open = props.open;
    const self = props.self;
    const loadAdminList = props.loadAdminList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const form = useRef();

    return (
        <Modal
            title="新增管理员"
            width="680px"
            destroyOnClose={true}
            footer={null}
            open={open}
            onOk={() => {
                if (loadAdminList) {
                    loadAdminList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadAdminList) {
                    loadAdminList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    form.current.setFieldsValue({
                        loginName: "",
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
                        post("/api/admin/user/add/save", {
                            loginName: values.loginName
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadAdminList) {
                                loadAdminList();
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

class AdminUserDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadAdminList = props.loadAdminList;

        this.state = {
            adminId: 0,
            open: false,
        };
    }

    show = (adminId) => {
        this.setState((pre) => {
            return {
                ...pre,
                adminId: adminId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                adminId: 0,
                open: false,
            };
        });
    }

    render() {
        const adminId = this.state.adminId;
        const open = this.state.open;
        const loadAdminList = this.loadAdminList;

        return (
            <AdminUserDetailInner
                adminId={adminId}
                open={open}
                self={this}
                loadAdminList={loadAdminList}
            />
        );
    }
}

export default AdminUserDetail;
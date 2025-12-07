import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ClientKeyDetailInner = (props) => {
    const clientKeyId = props.clientKeyId;
    const open = props.open;
    const self = props.self;
    const loadClientKeyList = props.loadClientKeyList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (clientKeyId === 0) {
        title.current = "客户端密钥新增";
    }

    if (clientKeyId > 0) {
        title.current = "客户端密钥编辑";
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
                if (loadClientKeyList) {
                    loadClientKeyList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadClientKeyList) {
                    loadClientKeyList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/client-key/detail", {
                        "id": clientKeyId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            collegeId: data.collegeId,
                            productId: data.productId,
                            keyType: data.keyType,
                            clientKey: data.clientKey,
                            userId: data.userId,
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
                        post("/api/client-key/save", {
                            id: clientKeyId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            keyType: values.keyType,
                            clientKey: values.clientKey,
                            userId: values.userId,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadClientKeyList) {
                                loadClientKeyList();
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
                        <Descriptions.Item label="学校">
                            <Form.Item
                                name="collegeId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入学校!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="学校"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="产品">
                            <Form.Item
                                name="productId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入产品!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="产品"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="类型">
                            <Form.Item
                                name="keyType"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入类型!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="类型"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="密钥">
                            <Form.Item
                                name="clientKey"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入密钥!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="密钥"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="操作人">
                            <Form.Item
                                name="userId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入操作人!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="操作人"
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

class ClientKeyDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadClientKeyList = props.loadClientKeyList;

        this.state = {
            clientKeyId: 0,
            open: false,
        };
    }

    show = (clientKeyId) => {
        this.setState((pre) => {
           return {
                ...pre,
                clientKeyId: clientKeyId,
                open: true,
           };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                clientKeyId: 0,
                open: false,
            };
        });
    }

    render() {
        const clientKeyId = this.state.clientKeyId;
        const open = this.state.open;
        const loadClientKeyList = this.loadClientKeyList;

        return (
            <ClientKeyDetailInner
                clientKeyId={clientKeyId}
                open={open}
                self={this}
                loadClientKeyList={loadClientKeyList}
            />
        )
    }
}

export default ClientKeyDetail;

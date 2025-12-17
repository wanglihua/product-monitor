import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";

const ProductUrlMonitorEntryDetailInner = (props) => {
    const productUrlMonitorEntryId = props.productUrlMonitorEntryId;
    const open = props.open;
    const self = props.self;
    const loadProductUrlMonitorEntryList = props.loadProductUrlMonitorEntryList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productUrlMonitorEntryId === 0) {
        title.current = "网站监测详情新增";
    }

    if (productUrlMonitorEntryId > 0) {
        title.current = "网站监测详情编辑";
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
                if (loadProductUrlMonitorEntryList) {
                    loadProductUrlMonitorEntryList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductUrlMonitorEntryList) {
                    loadProductUrlMonitorEntryList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-url-monitor-entry/detail", {
                        "id": productUrlMonitorEntryId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            productUrlMonitorId: data.productUrlMonitorId,
                            loginUser: data.loginUser,
                            loginPassword: data.loginPassword,
                            ipAddress: data.ipAddress,
                            provinceType: data.provinceType,
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
                    requiredMark={true}
                    onFinish={(values) => {
                        post("/api/product-url-monitor-entry/save", {
                            id: productUrlMonitorEntryId,
                            productUrlMonitorId: values.productUrlMonitorId,
                            loginUser: values.loginUser,
                            loginPassword: values.loginPassword,
                            ipAddress: values.ipAddress,
                            provinceType: values.provinceType,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadProductUrlMonitorEntryList) {
                                loadProductUrlMonitorEntryList();
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
                        <Descriptions.Item label="网站监测">
                            <Form.Item
                                name="productUrlMonitorId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入网站监测!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="网站监测"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="账号">
                            <Form.Item
                                name="loginUser"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入账号!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="账号"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="密码">
                            <Form.Item
                                name="loginPassword"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入密码!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="密码"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="使用地址">
                            <Form.Item
                                name="ipAddress"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入使用地址!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={32}
                                    placeholder="使用地址"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="省份">
                            <Form.Item
                                name="provinceType"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入省份!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="省份"
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

class ProductUrlMonitorEntryDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductUrlMonitorEntryList = props.loadProductUrlMonitorEntryList;

        this.state = {
            productUrlMonitorEntryId: 0,
            open: false,
        };
    }

    show = (productUrlMonitorEntryId) => {
        this.setState((pre) => {
           return {
                ...pre,
                productUrlMonitorEntryId: productUrlMonitorEntryId,
                open: true,
           };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productUrlMonitorEntryId: 0,
                open: false,
            };
        });
    }

    render() {
        const productUrlMonitorEntryId = this.state.productUrlMonitorEntryId;
        const open = this.state.open;
        const loadProductUrlMonitorEntryList = this.loadProductUrlMonitorEntryList;

        return (
            <ProductUrlMonitorEntryDetailInner
                productUrlMonitorEntryId={productUrlMonitorEntryId}
                open={open}
                self={this}
                loadProductUrlMonitorEntryList={loadProductUrlMonitorEntryList}
            />
        )
    }
}

export default ProductUrlMonitorEntryDetail;

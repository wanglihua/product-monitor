import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ProductUrlDetailInner = (props) => {
    const productUrlId = props.productUrlId;
    const open = props.open;
    const self = props.self;
    const loadProductUrlList = props.loadProductUrlList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productUrlId === 0) {
        title.current = "网址管理新增";
    }

    if (productUrlId > 0) {
        title.current = "网址管理编辑";
    }

    const [state, setState] = useState({});

    const [productUrlTypeOptions, setProductUrlTypeOptions] = useState([]);

    const form = useRef();

    return (
        <Modal
            title={title.current}
            width="680px"
            destroyOnClose={true}
            footer={null}
            open={open}
            onOk={() => {
                if (loadProductUrlList) {
                    loadProductUrlList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductUrlList) {
                    loadProductUrlList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-url/detail", {
                        "id": productUrlId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        let typeCode = undefined;
                        if (productUrlId > 0) {
                            typeCode = data.typeCode;
                        }

                        setProductUrlTypeOptions(data.productUrlTypeList)

                        form.current.setFieldsValue({
                            name: data.name,
                            url: data.url,
                            typeCode: typeCode,
                            loginUser: data.loginUser,
                            loginPassword: data.loginPassword,
                            description: data.description,
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
                        post("/api/product-url/save", {
                            id: productUrlId,
                            name: values.name,
                            url: values.url,
                            typeCode: values.typeCode,
                            loginUser: values.loginUser,
                            loginPassword: values.loginPassword,
                            description: values.description,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadProductUrlList) {
                                loadProductUrlList();
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
                        <Descriptions.Item label="产品名称">
                            <Form.Item
                                name="name"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入产品名称!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="产品名称"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="网址">
                            <Form.Item
                                name="url"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入网址!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={256}
                                    placeholder="网址"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="类型">
                            <Form.Item
                                name="typeCode"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入类型!',
                                    },
                                ]}
                            >
                                <Select
                                    placeholder="请选择类型"
                                    options={productUrlTypeOptions}
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

                        <Descriptions.Item label="备注">
                            <Form.Item
                                name="description"
                                rules={[]}
                            >
                                <Input.TextArea
                                    rows={3}
                                    maxLength={512}
                                    placeholder="备注"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                    </Descriptions>

                    <Form.Item style={{marginTop: token.formButtonMarginTop, textAlign: "right"}}>
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

class ProductUrlDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductUrlList = props.loadProductUrlList;

        this.state = {
            productUrlId: 0,
            open: false,
        };
    }

    show = (productUrlId) => {
        this.setState((pre) => {
            return {
                ...pre,
                productUrlId: productUrlId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productUrlId: 0,
                open: false,
            };
        });
    }

    render() {
        const productUrlId = this.state.productUrlId;
        const open = this.state.open;
        const loadProductUrlList = this.loadProductUrlList;

        return (
            <ProductUrlDetailInner
                productUrlId={productUrlId}
                open={open}
                self={this}
                loadProductUrlList={loadProductUrlList}
            />
        )
    }
}

export default ProductUrlDetail;

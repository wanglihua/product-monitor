import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";

const ProductVersionDetailInner = (props) => {
    const productVersionId = props.productVersionId;
    const productId = props.productId;
    const open = props.open;
    const self = props.self;
    const loadProductVersionList = props.loadProductVersionList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const [state, setState] = useState({
        productName: "",
    });

    const title = useRef("");
    if (productVersionId === 0) {
        title.current = `产品${state.productName} 版本新增`;
    }

    if (productVersionId > 0) {
        title.current = `$产品${state.productName} 版本编辑`;
    }

    const form = useRef();

    return (
        <Modal
            title={title.current}
            width="680px"
            destroyOnClose={true}
            footer={null}
            open={open}
            onOk={() => {
                if (loadProductVersionList) {
                    loadProductVersionList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductVersionList) {
                    loadProductVersionList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-version/detail", {
                        "productVersionId": productVersionId,
                        "productId": productId,
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            version: data.version,
                            versionUpdateBase: data.versionUpdateBase,
                            versionUpdateDesc: data.versionUpdateDesc,
                            versionUpdateTime: dayjs(data.versionUpdateTime),
                        });

                        setState((pre) => {
                            return {
                                ...pre,
                                productName: data.productName,
                            };
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
                        post("/api/product-version/save", {
                            id: productVersionId,
                            productId: productId,
                            version: values.version,
                            versionUpdateBase: values.versionUpdateBase,
                            versionUpdateDesc: values.versionUpdateDesc,
                            versionUpdateTime: values.versionUpdateTime,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadProductVersionList) {
                                loadProductVersionList();
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
                        <Descriptions.Item label="版本号">
                            <Form.Item
                                name="version"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入版本号!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={30}
                                    placeholder="版本号"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>

                        <Descriptions.Item label="更新基础">
                            <Form.Item
                                name="versionUpdateBase"
                            >
                                <Input
                                    maxLength={30}
                                    placeholder="更新基础"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>

                        <Descriptions.Item label="版本更新时间">
                            <Form.Item
                                name="versionUpdateTime"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入更新时间!',
                                    },
                                ]}
                            >
                                <DatePicker
                                    style={{width: "100%"}}
                                    placeholder="版本更新时间"
                                />
                            </Form.Item>
                        </Descriptions.Item>

                        <Descriptions.Item label="更新内容">
                            <Form.Item
                                name="versionUpdateDesc"
                            >
                                <Input.TextArea
                                    rows={2}
                                    maxLength={500}
                                    placeholder="更新内容"
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

class ProductVersionDetail extends React.Component {
    constructor(props) {
        super(props);

        const productId = props.productId;
        this.loadProductVersionList = props.loadProductVersionList;

        this.state = {
            productVersionId: 0,
            productId: productId,
            open: false,
        };
    }

    show = (productVersionId) => {
        this.setState((pre) => {
            return {
                ...pre,
                productVersionId: productVersionId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productVersionId: 0,
                open: false,
            };
        });
    }

    render() {
        const productVersionId = this.state.productVersionId;
        const productId = this.state.productId;
        const open = this.state.open;
        const loadProductVersionList = this.loadProductVersionList;

        return (
            <ProductVersionDetailInner
                productVersionId={productVersionId}
                productId={productId}
                open={open}
                self={this}
                loadProductVersionList={loadProductVersionList}
            />
        )
    }
}

export default ProductVersionDetail;
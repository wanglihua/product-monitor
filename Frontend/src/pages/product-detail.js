import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";

const ProductDetailInner = (props) => {
    const productId = props.productId;
    const open = props.open;
    const self = props.self;
    const loadProductList = props.loadProductList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productId === 0) {
        title.current = "产品新增";
    }

    if (productId > 0) {
        title.current = "产品编辑";
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
                if (loadProductList) {
                    loadProductList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductList) {
                    loadProductList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product/detail", {
                        "id": productId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            name: data.name,
                            englishName: data.englishName,
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
                        post("/api/product/save", {
                            id: productId,
                            name: values.name,
                            englishName: values.englishName,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadProductList) {
                                loadProductList();
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
                                    maxLength={30}
                                    placeholder="产品名称"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>

                        <Descriptions.Item label="产品英文名">
                            <Form.Item
                                name="englishName"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入产品英文名!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={30}
                                    placeholder="产品英文名"
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

class ProductDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductList = props.loadProductList;

        this.state = {
            productId: 0,
            open: false,
        };
    }

    show = (productId) => {
        this.setState((pre) => {
            return {
                ...pre,
                productId: productId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productId: 0,
                open: false,
            };
        });
    }

    render() {
        const productId = this.state.productId;
        const open = this.state.open;
        const loadProductList = this.loadProductList;

        return (
            <ProductDetailInner
                productId={productId}
                open={open}
                self={this}
                loadProductList={loadProductList}
            />
        )
    }
}

export default ProductDetail;
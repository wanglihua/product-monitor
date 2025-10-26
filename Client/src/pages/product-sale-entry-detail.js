import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ProductSaleEntryDetailInner = (props) => {
    const productSaleEntryId = props.productSaleEntryId;
    const productSaleId = props.productSaleId;
    const open = props.open;
    const self = props.self;
    const loadProductSaleList = props.loadProductSaleList;
    const loadProductSaleEntryList = props.loadProductSaleEntryList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productSaleEntryId === 0) {
        title.current = "销售目录明细新增";
    }

    if (productSaleEntryId > 0) {
        title.current = "销售目录明细编辑";
    }

    const [state, setState] = useState({});

    const form = useRef();

    return (
        <Modal
            title={title.current}
            width="880px"
            destroyOnClose={true}
            footer={null}
            open={open}
            onOk={() => {
                if (loadProductSaleList) {
                    loadProductSaleList();
                }

                if (loadProductSaleEntryList) {
                    loadProductSaleEntryList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductSaleList) {
                    loadProductSaleList();
                }

                if (loadProductSaleEntryList) {
                    loadProductSaleEntryList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-sale-entry/detail", {
                        "id": productSaleId
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
                            version: data.version,
                            saleDate: dayjs(data.saleDate),
                            serviceExpireDate: dayjs(data.serviceExpireDate),
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
                        post("/api/product-sale-entry/save", {
                            productSaleEntryId: productSaleEntryId,
                            productSaleId: productSaleId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            version: values.version,
                            saleDate: values.saleDate,
                            serviceExpireDate: values.serviceExpireDate,
                            description: values.description,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadProductSaleList) {
                                loadProductSaleList();
                            }

                            if (loadProductSaleEntryList) {
                                loadProductSaleEntryList();
                            }

                            self.hide();
                        });
                    }}
                >
                    <Descriptions
                        bordered={true}
                        layout="horizontal"
                        size="small"
                        column={2}
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
                                    maxLength={32}
                                    placeholder="版本号"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="销售时间">
                            <Form.Item
                                name="saleDate"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入销售时间!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="销售时间"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="服务到期时间">
                            <Form.Item
                                name="serviceExpireDate"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入服务到期时间!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="服务到期时间"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item>
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="备注">
                            <Form.Item
                                name="description"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入备注!',
                                    },
                                ]}
                            >
                                <Input.TextArea
                                    rows={2}
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

class ProductSaleEntryDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductSaleList = props.loadProductSaleList;
        this.loadProductSaleEntryList = props.loadProductSaleEntryList;

        this.state = {

            productSaleId: 0,
            open: false,
        };
    }

    show = (productSaleId) => {
        this.setState({
            productSaleId: productSaleId,
            open: true,
        });
    }

    hide = () => {
        this.setState({
            productSaleId: 0,
            open: false,
        });
    }

    render() {
        const productSaleId = this.state.productSaleId;
        const open = this.state.open;
        const loadProductSaleList = this.loadProductSaleList;
        const loadProductSaleEntryList = this.loadProductSaleEntryList;

        return (
            <ProductSaleEntryDetailInner
                productSaleId={productSaleId}
                open={open}
                self={this}
                loadProductSaleList={loadProductSaleList}
                loadProductSaleEntryList={loadProductSaleEntryList}
            />
        )
    }
}

export default ProductSaleEntryDetail;

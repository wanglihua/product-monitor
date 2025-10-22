import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ProductSaleDetailInner = (props) => {
    const productSaleId = props.productSaleId;
    const open = props.open;
    const self = props.self;
    const loadProductSaleList = props.loadProductSaleList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productSaleId === 0) {
        title.current = "销售目录新增";
    }

    if (productSaleId > 0) {
        title.current = "销售目录编辑";
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
                if (loadProductSaleList) {
                    loadProductSaleList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductSaleList) {
                    loadProductSaleList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-sale/detail", {
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
                            saleTimes: data.saleTimes,
                            installTimes: data.installTimes,
                            updateTimes: data.updateTimes,
                            serviceTimes: data.serviceTimes,
                            version: data.version,
                            saleDate: data.saleDate,
                            serviceExpireDate: data.serviceExpireDate,
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
                        post("/api/product-sale/save", {
                            id: productSaleId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            saleTimes: values.saleTimes,
                            installTimes: values.installTimes,
                            updateTimes: values.updateTimes,
                            serviceTimes: values.serviceTimes,
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
                        <Descriptions.Item label="销售次数">
                            <Form.Item
                                name="saleTimes"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入销售次数!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="销售次数"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="安装次数">
                            <Form.Item
                                name="installTimes"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入安装次数!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="安装次数"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="更新次数">
                            <Form.Item
                                name="updateTimes"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入更新次数!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="更新次数"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="售后次数">
                            <Form.Item
                                name="serviceTimes"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入售后次数!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="售后次数"
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
                                <Input
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

class ProductSaleDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductSaleList = props.loadProductSaleList;

        this.state = {
            productSaleId: -1,
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
            productSaleId: -1,
            open: false,
        });
    }

    render() {
        const productSaleId = this.state.productSaleId;
        const open = this.state.open;
        const loadProductSaleList = this.loadProductSaleList;

        return (
            <ProductSaleDetailInner
                productSaleId={productSaleId}
                open={open}
                self={this}
                loadProductSaleList={loadProductSaleList}
            />
        )
    }
}

export default ProductSaleDetail;

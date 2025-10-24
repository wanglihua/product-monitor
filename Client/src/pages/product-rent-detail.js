import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ProductRentDetailInner = (props) => {
    const productRentId = props.productRentId;
    const open = props.open;
    const self = props.self;
    const loadProductRentList = props.loadProductRentList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productRentId === 0) {
        title.current = "租赁目录新增";
    }

    if (productRentId > 0) {
        title.current = "租赁目录编辑";
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
                if (loadProductRentList) {
                    loadProductRentList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductRentList) {
                    loadProductRentList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-rent/detail", {
                        "id": productRentId
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
                            rentTimes: data.rentTimes,
                            serviceTimes: data.serviceTimes,
                            totalDays: data.totalDays,
                            remainDays: data.remainDays,
                            beginDate: data.beginDate,
                            endDate: data.endDate,
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
                        post("/api/product-rent/save", {
                            id: productRentId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            rentTimes: values.rentTimes,
                            serviceTimes: values.serviceTimes,
                            totalDays: values.totalDays,
                            remainDays: values.remainDays,
                            beginDate: values.beginDate,
                            endDate: values.endDate,
                            description: values.description,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadProductRentList) {
                                loadProductRentList();
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
                        <Descriptions.Item label="开始时间">
                            <Form.Item
                                name="beginDate"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入开始时间!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="开始时间"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="结束时间">
                            <Form.Item
                                name="endDate"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入结束时间!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="结束时间"
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

class ProductRentDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductRentList = props.loadProductRentList;

        this.state = {
            productRentId: -1,
            open: false,
        };
    }

    show = (productRentId) => {
        this.setState({
            productRentId: productRentId,
            open: true,
        });
    }

    hide = () => {
        this.setState({
            productRentId: -1,
            open: false,
        });
    }

    render() {
        const productRentId = this.state.productRentId;
        const open = this.state.open;
        const loadProductRentList = this.loadProductRentList;

        return (
            <ProductRentDetailInner
                productRentId={productRentId}
                open={open}
                self={this}
                loadProductRentList={loadProductRentList}
            />
        )
    }
}

export default ProductRentDetail;

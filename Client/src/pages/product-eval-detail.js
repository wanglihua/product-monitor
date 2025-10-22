import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ProductEvalDetailInner = (props) => {
    const productEvalId = props.productEvalId;
    const open = props.open;
    const self = props.self;
    const loadProductEvalList = props.loadProductEvalList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productEvalId === 0) {
        title.current = "试用目录新增";
    }

    if (productEvalId > 0) {
        title.current = "试用目录编辑";
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
                if (loadProductEvalList) {
                    loadProductEvalList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductEvalList) {
                    loadProductEvalList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-eval/detail", {
                        "id": productEvalId
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
                            evalTimes: data.evalTimes,
                            saleTimes: data.saleTimes,
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
                        post("/api/product-eval/save", {
                            id: productEvalId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            evalTimes: values.evalTimes,
                            saleTimes: values.saleTimes,
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

                            if (loadProductEvalList) {
                                loadProductEvalList();
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
                        <Descriptions.Item label="试用次数">
                            <Form.Item
                                name="evalTimes"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入试用次数!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="试用次数"
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
                        <Descriptions.Item label="累计天数">
                            <Form.Item
                                name="totalDays"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入累计天数!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="累计天数"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="剩余天数">
                            <Form.Item
                                name="remainDays"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入剩余天数!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="剩余天数"
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

class ProductEvalDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductEvalList = props.loadProductEvalList;

        this.state = {
            productEvalId: -1,
            open: false,
        };
    }

    show = (productEvalId) => {
        this.setState({
            productEvalId: productEvalId,
            open: true,
        });
    }

    hide = () => {
        this.setState({
            productEvalId: -1,
            open: false,
        });
    }

    render() {
        const productEvalId = this.state.productEvalId;
        const open = this.state.open;
        const loadProductEvalList = this.loadProductEvalList;

        return (
            <ProductEvalDetailInner
                productEvalId={productEvalId}
                open={open}
                self={this}
                loadProductEvalList={loadProductEvalList}
            />
        )
    }
}

export default ProductEvalDetail;

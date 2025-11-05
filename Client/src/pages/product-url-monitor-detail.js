import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ProductUrlMonitorDetailInner = (props) => {
    const productUrlMonitorId = props.productUrlMonitorId;
    const open = props.open;
    const self = props.self;
    const loadProductUrlMonitorList = props.loadProductUrlMonitorList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productUrlMonitorId === 0) {
        title.current = "网站监测新增";
    }

    if (productUrlMonitorId > 0) {
        title.current = "网站监测编辑";
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
                if (loadProductUrlMonitorList) {
                    loadProductUrlMonitorList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductUrlMonitorList) {
                    loadProductUrlMonitorList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-url-monitor/detail", {
                        "id": productUrlMonitorId
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
                            productUrlType: data.productUrlType,
                            taskCount: data.taskCount,
                            teacherCount: data.teacherCount,
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
                        post("/api/product-url-monitor/save", {
                            id: productUrlMonitorId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            productUrlType: values.productUrlType,
                            taskCount: values.taskCount,
                            teacherCount: values.teacherCount,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadProductUrlMonitorList) {
                                loadProductUrlMonitorList();
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
                        <Descriptions.Item label="分类">
                            <Form.Item
                                name="productUrlType"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入分类!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="分类"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="任务数量">
                            <Form.Item
                                name="taskCount"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入任务数量!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="任务数量"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="教师数量">
                            <Form.Item
                                name="teacherCount"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入教师数量!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="教师数量"
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

class ProductUrlMonitorDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductUrlMonitorList = props.loadProductUrlMonitorList;

        this.state = {
            productUrlMonitorId: 0,
            open: false,
        };
    }

    show = (productUrlMonitorId) => {
        this.setState((pre) => {
           return {
                ...pre,
                productUrlMonitorId: productUrlMonitorId,
                open: true,
           };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productUrlMonitorId: 0,
                open: false,
            };
        });
    }

    render() {
        const productUrlMonitorId = this.state.productUrlMonitorId;
        const open = this.state.open;
        const loadProductUrlMonitorList = this.loadProductUrlMonitorList;

        return (
            <ProductUrlMonitorDetailInner
                productUrlMonitorId={productUrlMonitorId}
                open={open}
                self={this}
                loadProductUrlMonitorList={loadProductUrlMonitorList}
            />
        )
    }
}

export default ProductUrlMonitorDetail;

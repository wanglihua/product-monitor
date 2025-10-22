import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ProductServiceDetailInner = (props) => {
    const productServiceId = props.productServiceId;
    const open = props.open;
    const self = props.self;
    const loadProductServiceList = props.loadProductServiceList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productServiceId === 0) {
        title.current = "售后服务新增";
    }

    if (productServiceId > 0) {
        title.current = "售后服务编辑";
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
                if (loadProductServiceList) {
                    loadProductServiceList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductServiceList) {
                    loadProductServiceList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-service/detail", {
                        "id": productServiceId
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
                            projectTypeCode: data.projectTypeCode,
                            serviceDate: data.serviceDate,
                            serviceCount: data.serviceCount,
                            serviceUrl: data.serviceUrl,
                            loginUser: data.loginUser,
                            loginPassword: data.loginPassword,
                            contactTeacher: data.contactTeacher,
                            contactMobile: data.contactMobile,
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
                        post("/api/product-service/save", {
                            id: productServiceId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            projectTypeCode: values.projectTypeCode,
                            serviceDate: values.serviceDate,
                            serviceCount: values.serviceCount,
                            serviceUrl: values.serviceUrl,
                            loginUser: values.loginUser,
                            loginPassword: values.loginPassword,
                            contactTeacher: values.contactTeacher,
                            contactMobile: values.contactMobile,
                            description: values.description,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadProductServiceList) {
                                loadProductServiceList();
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
                        <Descriptions.Item label="项目类型">
                            <Form.Item
                                name="projectTypeCode"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入项目类型!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="项目类型"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="售后时间">
                            <Form.Item
                                name="serviceDate"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入售后时间!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="售后时间"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="售后次数">
                            <Form.Item
                                name="serviceCount"
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
                        <Descriptions.Item label="售后网址">
                            <Form.Item
                                name="serviceUrl"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入售后网址!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={256}
                                    placeholder="售后网址"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="售后账号">
                            <Form.Item
                                name="loginUser"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入售后账号!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="售后账号"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="售后密码">
                            <Form.Item
                                name="loginPassword"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入售后密码!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="售后密码"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="联系老师">
                            <Form.Item
                                name="contactTeacher"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入联系老师!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={32}
                                    placeholder="联系老师"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="联系电话">
                            <Form.Item
                                name="contactMobile"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入联系电话!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={32}
                                    placeholder="联系电话"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="售后问题">
                            <Form.Item
                                name="description"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入售后问题!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={512}
                                    placeholder="售后问题"
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

class ProductServiceDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductServiceList = props.loadProductServiceList;

        this.state = {
            productServiceId: -1,
            open: false,
        };
    }

    show = (productServiceId) => {
        this.setState({
            productServiceId: productServiceId,
            open: true,
        });
    }

    hide = () => {
        this.setState({
            productServiceId: -1,
            open: false,
        });
    }

    render() {
        const productServiceId = this.state.productServiceId;
        const open = this.state.open;
        const loadProductServiceList = this.loadProductServiceList;

        return (
            <ProductServiceDetailInner
                productServiceId={productServiceId}
                open={open}
                self={this}
                loadProductServiceList={loadProductServiceList}
            />
        )
    }
}

export default ProductServiceDetail;

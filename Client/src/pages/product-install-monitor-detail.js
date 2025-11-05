import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ProductInstallMonitorDetailInner = (props) => {
    const productInstallMonitorId = props.productInstallMonitorId;
    const open = props.open;
    const self = props.self;
    const loadProductInstallMonitorList = props.loadProductInstallMonitorList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productInstallMonitorId === 0) {
        title.current = "安装监测新增";
    }

    if (productInstallMonitorId > 0) {
        title.current = "安装监测编辑";
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
                if (loadProductInstallMonitorList) {
                    loadProductInstallMonitorList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductInstallMonitorList) {
                    loadProductInstallMonitorList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-install-monitor/detail", {
                        "id": productInstallMonitorId
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
                            provinceType: data.provinceType,
                            cpuSerialNo: data.cpuSerialNo,
                            netMacAddress: data.netMacAddress,
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
                        post("/api/product-install-monitor/save", {
                            id: productInstallMonitorId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            provinceType: values.provinceType,
                            cpuSerialNo: values.cpuSerialNo,
                            netMacAddress: values.netMacAddress,
                            taskCount: values.taskCount,
                            teacherCount: values.teacherCount,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadProductInstallMonitorList) {
                                loadProductInstallMonitorList();
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
                        <Descriptions.Item label="CPU序列号">
                            <Form.Item
                                name="cpuSerialNo"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入CPU序列号!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="CPU序列号"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="网卡硬件地址">
                            <Form.Item
                                name="netMacAddress"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入网卡硬件地址!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="网卡硬件地址"
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

class ProductInstallMonitorDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductInstallMonitorList = props.loadProductInstallMonitorList;

        this.state = {
            productInstallMonitorId: 0,
            open: false,
        };
    }

    show = (productInstallMonitorId) => {
        this.setState((pre) => {
           return {
                ...pre,
                productInstallMonitorId: productInstallMonitorId,
                open: true,
           };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productInstallMonitorId: 0,
                open: false,
            };
        });
    }

    render() {
        const productInstallMonitorId = this.state.productInstallMonitorId;
        const open = this.state.open;
        const loadProductInstallMonitorList = this.loadProductInstallMonitorList;

        return (
            <ProductInstallMonitorDetailInner
                productInstallMonitorId={productInstallMonitorId}
                open={open}
                self={this}
                loadProductInstallMonitorList={loadProductInstallMonitorList}
            />
        )
    }
}

export default ProductInstallMonitorDetail;

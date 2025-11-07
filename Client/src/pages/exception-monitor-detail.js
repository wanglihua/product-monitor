import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ExceptionMonitorDetailInner = (props) => {
    const exceptionMonitorId = props.exceptionMonitorId;
    const open = props.open;
    const self = props.self;
    const loadExceptionMonitorList = props.loadExceptionMonitorList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (exceptionMonitorId === 0) {
        title.current = "异常检测新增";
    }

    if (exceptionMonitorId > 0) {
        title.current = "异常检测编辑";
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
                if (loadExceptionMonitorList) {
                    loadExceptionMonitorList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadExceptionMonitorList) {
                    loadExceptionMonitorList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/exception-monitor/detail", {
                        "id": exceptionMonitorId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            productId: data.productId,
                            ipAddress: data.ipAddress,
                            provinceType: data.provinceType,
                            port: data.port,
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
                    }}
                >
                    <Descriptions
                        bordered={true}
                        layout="horizontal"
                        size="small"
                        column={1}
                        labelStyle={{width: "120px"}}
                    >
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
                        <Descriptions.Item label="使用IP">
                            <Form.Item
                                name="ipAddress"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入使用IP!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={32}
                                    placeholder="使用IP"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="使用省份">
                            <Form.Item
                                name="provinceType"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入使用省份!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="使用省份"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="使用端口">
                            <Form.Item
                                name="port"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入使用端口!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="使用端口"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                    </Descriptions>

                    <Form.Item style={{marginTop: token.formButtonMarginTop, textAlign: "right"}}>
                        <Space>
                            <Button type="default" size="large" onClick={() => {
                                self.hide();
                            }}>
                                关闭
                            </Button>
                        </Space>
                    </Form.Item>
                </Form>

            </ConfigProvider>
        </Modal>
    );
};

class ExceptionMonitorDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadExceptionMonitorList = props.loadExceptionMonitorList;

        this.state = {
            exceptionMonitorId: 0,
            open: false,
        };
    }

    show = (exceptionMonitorId) => {
        this.setState((pre) => {
            return {
                ...pre,
                exceptionMonitorId: exceptionMonitorId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                exceptionMonitorId: 0,
                open: false,
            };
        });
    }

    render() {
        const exceptionMonitorId = this.state.exceptionMonitorId;
        const open = this.state.open;
        const loadExceptionMonitorList = this.loadExceptionMonitorList;

        return (
            <ExceptionMonitorDetailInner
                exceptionMonitorId={exceptionMonitorId}
                open={open}
                self={this}
                loadExceptionMonitorList={loadExceptionMonitorList}
            />
        )
    }
}

export default ExceptionMonitorDetail;

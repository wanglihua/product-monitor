import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";
import {CloseCircleOutlined} from "@ant-design/icons";

const ProductInstallMonitorEntryDetailInner = (props) => {
    const productInstallMonitorEntryId = props.productInstallMonitorEntryId;
    const open = props.open;
    const self = props.self;
    const loadProductInstallMonitorEntryList = props.loadProductInstallMonitorEntryList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productInstallMonitorEntryId === 0) {
        title.current = "院校使用详情新增";
    }

    if (productInstallMonitorEntryId > 0) {
        title.current = "院校使用详情编辑";
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
                if (loadProductInstallMonitorEntryList) {
                    loadProductInstallMonitorEntryList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductInstallMonitorEntryList) {
                    loadProductInstallMonitorEntryList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-install-monitor-entry/detail", {
                        "id": productInstallMonitorEntryId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            productInstallMonitorId: data.productInstallMonitorId,
                            loginUser: data.loginUser,
                            accountType: data.accountType,
                            ipAddress: data.ipAddress,
                            provinceType: data.provinceType,
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
                >
                    <Descriptions
                        bordered={true}
                        layout="horizontal"
                        size="small"
                        column={1}
                        labelStyle={{width: "120px"}}
                    >
                        <Descriptions.Item label="安装监测">
                            <Form.Item
                                name="productInstallMonitorId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入安装监测!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="安装监测"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="使用账号">
                            <Form.Item
                                name="loginUser"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入使用账号!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="使用账号"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="账号分类">
                            <Form.Item
                                name="accountType"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入账号分类!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder="账号分类"
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
                    </Descriptions>

                    <Form.Item style={{marginTop: token.formButtonMarginTop, textAlign: "right"}}>
                        <Space>
                            <Button type="default" size="large" onClick={() => {
                                self.hide();
                            }}>
                                <CloseCircleOutlined/>关闭
                            </Button>
                        </Space>
                    </Form.Item>
                </Form>

            </ConfigProvider>
        </Modal>
    );
};

class ProductInstallMonitorEntryDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductInstallMonitorEntryList = props.loadProductInstallMonitorEntryList;

        this.state = {
            productInstallMonitorEntryId: 0,
            open: false,
        };
    }

    show = (productInstallMonitorEntryId) => {
        this.setState((pre) => {
            return {
                ...pre,
                productInstallMonitorEntryId: productInstallMonitorEntryId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productInstallMonitorEntryId: 0,
                open: false,
            };
        });
    }

    render() {
        const productInstallMonitorEntryId = this.state.productInstallMonitorEntryId;
        const open = this.state.open;
        const loadProductInstallMonitorEntryList = this.loadProductInstallMonitorEntryList;

        return (
            <ProductInstallMonitorEntryDetailInner
                productInstallMonitorEntryId={productInstallMonitorEntryId}
                open={open}
                self={this}
                loadProductInstallMonitorEntryList={loadProductInstallMonitorEntryList}
            />
        )
    }
}

export default ProductInstallMonitorEntryDetail;

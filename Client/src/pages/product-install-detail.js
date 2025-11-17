import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";

const ProductInstallDetailInner = (props) => {
    const productInstallId = props.productInstallId;
    const open = props.open;
    const self = props.self;
    const loadProductInstallList = props.loadProductInstallList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productInstallId === 0) {
        title.current = "产品安装新增";
    }

    if (productInstallId > 0) {
        title.current = "产品安装编辑";
    }

    const [state, setState] = useState({});

    const [collegeSelectOptions, setCollegeSelectOptions] = useState([]);
    const [productSelectOptions, setProductSelectOptions] = useState([]);

    const form = useRef();

    return (
        <Modal
            title={title.current}
            width="980px"
            destroyOnClose={true}
            footer={null}
            open={open}
            onOk={() => {
                if (loadProductInstallList) {
                    loadProductInstallList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductInstallList) {
                    loadProductInstallList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-install/detail", {
                        "id": productInstallId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        let collegeId = undefined;
                        let productId = undefined;

                        if (productInstallId > 0) {
                            collegeId = data.collegeId;
                            setCollegeSelectOptions([{
                                value: collegeId,
                                label: data.collegeName,
                            }]);

                            productId = data.productId;
                            setProductSelectOptions([{
                                value: productId,
                                label: data.productName,
                            }]);
                        }

                        form.current.setFieldsValue({
                            collegeId: collegeId,
                            productId: productId,
                            versionSale: data.versionSale,
                            versionInstall: data.versionInstall,
                            installDate: dayjs(data.installDate),
                            updateUrl: data.updateUrl,
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
                        post("/api/product-install/save", {
                            id: productInstallId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            versionSale: values.versionSale,
                            versionInstall: values.versionInstall,
                            installDate: values.installDate,
                            updateUrl: values.updateUrl,
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

                            if (loadProductInstallList) {
                                loadProductInstallList();
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
                                        message: '请输入搜索条件, 并选择学校!',
                                    },
                                ]}
                            >
                                <Select
                                    showSearch={true}
                                    filterOption={false}
                                    notFoundContent={"暂无数据, 请输入正确的搜索条件"}
                                    placeholder="请输入搜索条件, 并选择学校"
                                    options={collegeSelectOptions}
                                    onSearch={(search) => {
                                        post("/api/college/select/data", {
                                            search: search,
                                        }, function (response) {
                                            const res = response.data;
                                            if (res.success !== true) {
                                                showError(res.message);
                                                return;
                                            }

                                            setCollegeSelectOptions(res.collegeList);
                                        });
                                    }}
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="产品">
                            <Form.Item
                                name="productId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入搜索条件, 并选择产品!',
                                    },
                                ]}
                            >
                                <Select
                                    showSearch={true}
                                    filterOption={false}
                                    notFoundContent={"暂无数据, 请输入正确的搜索条件"}
                                    placeholder="请输入搜索条件, 并选择产品"
                                    options={productSelectOptions}
                                    onSearch={(search) => {
                                        post("/api/product/select/data", {
                                            search: search,
                                        }, function (response) {
                                            const res = response.data;
                                            if (res.success !== true) {
                                                showError(res.message);
                                                return;
                                            }

                                            setProductSelectOptions(res.productList);
                                        });
                                    }}
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="销售版本">
                            <Form.Item
                                name="versionSale"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入销售版本!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={32}
                                    placeholder="销售版本"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="安装版本">
                            <Form.Item
                                name="versionInstall"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入安装版本!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={32}
                                    placeholder="安装版本"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="安装时间">
                            <Form.Item
                                name="installDate"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入安装时间!',
                                    },
                                ]}
                            >
                                <DatePicker
                                    style={{width: "100%"}}
                                    placeholder="安装时间"
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="更新网址">
                            <Form.Item
                                name="updateUrl"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入更新网址!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={256}
                                    placeholder="更新网址"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="系统账号">
                            <Form.Item
                                name="loginUser"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入系统账号!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="系统账号"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="系统密码">
                            <Form.Item
                                name="loginPassword"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入系统密码!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="系统密码"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="联系教师">
                            <Form.Item
                                name="contactTeacher"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入联系教师!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={32}
                                    placeholder="联系教师"
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
                        <Descriptions.Item label="其他备注">
                            <Form.Item
                                name="description"
                                rules={[]}
                            >
                                <Input.TextArea
                                    rows={2}
                                    maxLength={512}
                                    placeholder="其他备注"
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

class ProductInstallDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductInstallList = props.loadProductInstallList;

        this.state = {
            productInstallId: 0,
            open: false,
        };
    }

    show = (productInstallId) => {
        this.setState((pre) => {
            return {
                ...pre,
                productInstallId: productInstallId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productInstallId: 0,
                open: false,
            };
        });
    }

    render() {
        const productInstallId = this.state.productInstallId;
        const open = this.state.open;
        const loadProductInstallList = this.loadProductInstallList;

        return (
            <ProductInstallDetailInner
                productInstallId={productInstallId}
                open={open}
                self={this}
                loadProductInstallList={loadProductInstallList}
            />
        )
    }
}

export default ProductInstallDetail;

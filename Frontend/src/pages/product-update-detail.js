import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";

const ProductUpdateDetailInner = (props) => {
    const productUpdateId = props.productUpdateId;
    const open = props.open;
    const self = props.self;
    const loadProductUpdateList = props.loadProductUpdateList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productUpdateId === 0) {
        title.current = "产品更新新增";
    }

    if (productUpdateId > 0) {
        title.current = "产品更新编辑";
    }

    const [state, setState] = useState({});

    const [collegeSelectOptions, setCollegeSelectOptions] = useState([]);
    const [productSelectOptions, setProductSelectOptions] = useState([]);
    const [versionSelectOptions, setVersionSelectOptions] = useState([]);

    const form = useRef();

    return (
        <Modal
            title={title.current}
            width="980px"
            destroyOnClose={true}
            footer={null}
            open={open}
            onOk={() => {
                if (loadProductUpdateList) {
                    loadProductUpdateList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductUpdateList) {
                    loadProductUpdateList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-update/detail", {
                        "id": productUpdateId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        let collegeId = undefined;
                        let productId = undefined;

                        if (productUpdateId > 0) {
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
                            versionUpdate: data.versionUpdate,
                            updateUrl: data.updateUrl,
                            loginUser: data.loginUser,
                            loginPassword: data.loginPassword,
                            updateDate: dayjs(data.updateDate),
                            salesManager: data.salesManager,
                            contactTeacher: data.contactTeacher,
                            contactMobile: data.contactMobile,
                            description: data.description,
                        });

                        setVersionSelectOptions(data.productVersionList);
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
                    requiredMark={true}
                    onFinish={(values) => {
                        post("/api/product-update/save", {
                            id: productUpdateId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            versionSale: values.versionSale,
                            versionUpdate: values.versionUpdate,
                            updateUrl: values.updateUrl,
                            loginUser: values.loginUser,
                            loginPassword: values.loginPassword,
                            updateDate: values.updateDate,
                            salesManager: values.salesManager,
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

                            if (loadProductUpdateList) {
                                loadProductUpdateList();
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
                                    onChange={(value) => {
                                        post("/api/product/select/by/sale", {
                                            collegeId: value,
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
                        <Descriptions.Item label="产品">
                            <Form.Item
                                name="productId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请选择产品!',
                                    },
                                ]}
                            >
                                <Select
                                    showSearch={true}
                                    filterOption={false}
                                    notFoundContent={"暂无数据, 请先选择院校"}
                                    placeholder="请选择产品"
                                    options={productSelectOptions}
                                    onChange={(value) => {
                                        post("/api/product/version/select/data", {
                                            productId: value,
                                        }, function (response) {
                                            const res = response.data;
                                            if (res.success !== true) {
                                                showError(res.message);
                                                return;
                                            }

                                            setVersionSelectOptions(res.productVersionList);
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
                                        message: '请选择销售版本!',
                                    },
                                ]}
                            >
                                <Select
                                    placeholder="请选择销售版本!"
                                    options={versionSelectOptions}
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="更新版本">
                            <Form.Item
                                name="versionUpdate"
                                rules={[
                                    {
                                        required: true,
                                        message: '请选择更新版本!',
                                    },
                                ]}
                            >
                                <Select
                                    placeholder="请选择更新版本!"
                                    options={versionSelectOptions}
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
                        <Descriptions.Item label="">
                            <Form.Item></Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="更新账号">
                            <Form.Item
                                name="loginUser"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入更新账号!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="更新账号"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="更新密码">
                            <Form.Item
                                name="loginPassword"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入更新密码!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="更新密码"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="更新时间">
                            <Form.Item
                                name="updateDate"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入更新时间!',
                                    },
                                ]}
                            >
                                <DatePicker
                                    style={{width: "100%"}}
                                    placeholder="更新时间"
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="业务经理">
                            <Form.Item
                                name="salesManager"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入业务经理!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={32}
                                    placeholder="业务经理"
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
                        <Descriptions.Item label="其他备注">
                            <Form.Item
                                name="description"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入其他备注!',
                                    },
                                ]}
                            >
                                <Input.TextArea
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

class ProductUpdateDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductUpdateList = props.loadProductUpdateList;

        this.state = {
            productUpdateId: 0,
            open: false,
        };
    }

    show = (productUpdateId) => {
        this.setState((pre) => {
            return {
                ...pre,
                productUpdateId: productUpdateId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productUpdateId: 0,
                open: false,
            };
        });
    }

    render() {
        const productUpdateId = this.state.productUpdateId;
        const open = this.state.open;
        const loadProductUpdateList = this.loadProductUpdateList;

        return (
            <ProductUpdateDetailInner
                productUpdateId={productUpdateId}
                open={open}
                self={this}
                loadProductUpdateList={loadProductUpdateList}
            />
        )
    }
}

export default ProductUpdateDetail;

import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";

const ProductSpecialServiceDetailInner = (props) => {
    const productSpecialServiceId = props.productSpecialServiceId;
    const open = props.open;
    const self = props.self;
    const loadProductSpecialServiceList = props.loadProductSpecialServiceList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productSpecialServiceId === 0) {
        title.current = "特殊售后新增";
    }

    if (productSpecialServiceId > 0) {
        title.current = "特殊售后编辑";
    }

    const [state, setState] = useState({});

    const [projectTypeOptions, setProjectTypeOptions] = useState([]);
    const [collegeSelectOptions, setCollegeSelectOptions] = useState([]);
    const [productSelectOptions, setProductSelectOptions] = useState([]);

    const form = useRef();

    const projectTypeRef = useRef();
    const collegeRef = useRef();

    const loadProductList = () => {
        const projectTypeId = projectTypeRef.current;
        const collegeId = collegeRef.current;

        post("/api/product/select/by/project/type", {
            projectTypeId: projectTypeId,
            collegeId: collegeId,
        }, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setProductSelectOptions(res.productList);
        });
    };

    return (
        <Modal
            title={title.current}
            width="980px"
            destroyOnClose={true}
            footer={null}
            open={open}
            onOk={() => {
                if (loadProductSpecialServiceList) {
                    loadProductSpecialServiceList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductSpecialServiceList) {
                    loadProductSpecialServiceList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-special-service/detail", {
                        "id": productSpecialServiceId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        let projectTypeCode = undefined;
                        let collegeId = undefined;
                        let productId = undefined;

                        if (productSpecialServiceId > 0) {
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

                            projectTypeCode = data.projectTypeCode;
                        }

                        projectTypeRef.current = projectTypeCode;
                        collegeRef.current = collegeId;

                        setProductSelectOptions(data.productList);
                        setProjectTypeOptions(data.projectTypeList)

                        form.current.setFieldsValue({
                            collegeId: collegeId,
                            productId: productId,
                            projectTypeCode: projectTypeCode,
                            serviceDate: dayjs(data.serviceDate),
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
                    requiredMark={true}
                    onFinish={(values) => {
                        post("/api/product-special-service/save", {
                            id: productSpecialServiceId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            projectTypeCode: values.projectTypeCode,
                            serviceDate: values.serviceDate,
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

                            if (loadProductSpecialServiceList) {
                                loadProductSpecialServiceList();
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
                                <Select
                                    placeholder="请选择项目类型"
                                    options={projectTypeOptions}
                                    onChange={(value) => {
                                        projectTypeRef.current = value;
                                        loadProductList();
                                    }}
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item></Form.Item>
                        </Descriptions.Item>
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
                                        collegeRef.current = value;
                                        loadProductList();
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
                                        message: '请选择产品',
                                    },
                                ]}
                            >
                                <Select
                                    filterOption={false}
                                    notFoundContent={"暂无数据, 请先选择项目类型和院校"}
                                    placeholder="请选择产品"
                                    options={productSelectOptions}
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
                                <DatePicker
                                    style={{width: "100%"}}
                                    placeholder="售后时间"
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
                                        required: false,
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
                                        required: false,
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
                                        required: false,
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
                                        required: false,
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
                                <Input.TextArea
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

class ProductSpecialServiceDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadProductSpecialServiceList = props.loadProductSpecialServiceList;

        this.state = {
            productSpecialServiceId: 0,
            open: false,
        };
    }

    show = (productSpecialServiceId) => {
        this.setState((pre) => {
            return {
                ...pre,
                productSpecialServiceId: productSpecialServiceId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productSpecialServiceId: 0,
                open: false,
            };
        });
    }

    render() {
        const productSpecialServiceId = this.state.productSpecialServiceId;
        const open = this.state.open;
        const loadProductSpecialServiceList = this.loadProductSpecialServiceList;

        return (
            <ProductSpecialServiceDetailInner
                productSpecialServiceId={productSpecialServiceId}
                open={open}
                self={this}
                loadProductSpecialServiceList={loadProductSpecialServiceList}
            />
        )
    }
}

export default ProductSpecialServiceDetail;

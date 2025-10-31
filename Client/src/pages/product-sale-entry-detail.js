import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ProductSaleEntryDetailInner = (props) => {
    const productSaleEntryId = props.productSaleEntryId;
    const productSaleId = props.productSaleId;
    const open = props.open;
    const self = props.self;
    const loadProductSaleList = props.loadProductSaleList;
    const loadProductSaleEntryList = props.loadProductSaleEntryList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productSaleEntryId === 0) {
        title.current = "销售目录明细新增";
    }

    if (productSaleEntryId > 0) {
        title.current = "销售目录明细编辑";
    }

    const [state, setState] = useState({
        collegeSelectDisabled: false,
        productSelectDisabled: false,
    });

    const [collegeSelectOptions, setCollegeSelectOptions] = useState([]);
    const [productSelectOptions, setProductSelectOptions] = useState([]);

    const form = useRef();

    return (
        <Modal
            title={title.current}
            width="880px"
            destroyOnClose={true}
            footer={null}
            open={open}
            onOk={() => {
                if (loadProductSaleList) {
                    loadProductSaleList();
                }

                if (loadProductSaleEntryList) {
                    loadProductSaleEntryList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductSaleList) {
                    loadProductSaleList();
                }

                if (loadProductSaleEntryList) {
                    loadProductSaleEntryList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-sale-entry/detail", {
                        "productSaleEntryId": productSaleEntryId,
                        "productSaleId": productSaleId,
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        let collegeId = undefined;
                        let productId = undefined;

                        if (productSaleEntryId > 0 || productSaleId > 0) {
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

                            setState((pre) => {
                                return {
                                    ...pre,
                                    collegeSelectDisabled: true,
                                    productSelectDisabled: true,
                                };
                            });
                        }

                        form.current.setFieldsValue({
                            collegeId: collegeId,
                            productId: productId,
                            version: data.version,
                            saleDate: dayjs(data.saleDate),
                            serviceExpireDate: dayjs(data.serviceExpireDate),
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
                        post("/api/product-sale-entry/save", {
                            productSaleEntryId: productSaleEntryId,
                            productSaleId: productSaleId,
                            collegeId: values.collegeId,
                            productId: values.productId,
                            version: values.version,
                            saleDate: values.saleDate,
                            serviceExpireDate: values.serviceExpireDate,
                            description: values.description,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadProductSaleList) {
                                loadProductSaleList();
                            }

                            if (loadProductSaleEntryList) {
                                loadProductSaleEntryList();
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
                                        message: '请搜索选择学校!',
                                    },
                                ]}
                            >
                                <Select
                                    disabled={state.collegeSelectDisabled}
                                    showSearch={true}
                                    filterOption={false}
                                    notFoundContent={"暂无数据, 请输入正确的搜索条件"}
                                    placeholder="请搜索选择学校"
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
                                        message: '请搜索选择产品!',
                                    },
                                ]}
                            >
                                <Select
                                    disabled={state.productSelectDisabled}
                                    showSearch={true}
                                    filterOption={false}
                                    notFoundContent={"暂无数据, 请输入正确的搜索条件"}
                                    placeholder="请搜索选择产品"
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
                        <Descriptions.Item label="版本号">
                            <Form.Item
                                name="version"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入版本号!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={32}
                                    placeholder="版本号"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="销售时间">
                            <Form.Item
                                name="saleDate"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入销售时间!',
                                    },
                                ]}
                            >
                                <DatePicker
                                    style={{width: "100%"}}
                                    placeholder="销售时间"
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="服务到期时间">
                            <Form.Item
                                name="serviceExpireDate"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入服务到期时间!',
                                    },
                                ]}
                            >
                                <DatePicker
                                    style={{width: "100%"}}
                                    placeholder="服务到期时间"
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item>
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="备注">
                            <Form.Item
                                name="description"
                                rules={[]}
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

class ProductSaleEntryDetail extends React.Component {
    constructor(props) {
        super(props);

        const productSaleId = props.productSaleId;
        this.loadProductSaleList = props.loadProductSaleList;
        this.loadProductSaleEntryList = props.loadProductSaleEntryList;

        this.state = {
            productSaleEntryId: 0,
            productSaleId: productSaleId,
            open: false,
        };
    }

    show = (productSaleEntryId) => {
        this.setState((pre) => {
            return {
                ...pre,
                productSaleEntryId: productSaleEntryId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productSaleEntryId: 0,
                open: false,
            };
        });
    }

    render() {
        const productSaleEntryId = this.state.productSaleEntryId;
        const productSaleId = this.state.productSaleId;
        const open = this.state.open;
        const loadProductSaleList = this.loadProductSaleList;
        const loadProductSaleEntryList = this.loadProductSaleEntryList;

        return (
            <ProductSaleEntryDetailInner
                productSaleEntryId={productSaleEntryId}
                productSaleId={productSaleId}
                open={open}
                self={this}
                loadProductSaleList={loadProductSaleList}
                loadProductSaleEntryList={loadProductSaleEntryList}
            />
        )
    }
}

export default ProductSaleEntryDetail;

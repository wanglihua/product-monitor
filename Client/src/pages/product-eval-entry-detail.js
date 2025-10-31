import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const ProductEvalEntryDetailInner = (props) => {
    const productEvalEntryId = props.productEvalEntryId;
    const productEvalId = props.productEvalId;
    const open = props.open;
    const self = props.self;
    const loadProductEvalList = props.loadProductEvalList;
    const loadProductEvalEntryList = props.loadProductEvalEntryList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (productEvalEntryId === 0) {
        title.current = "试用目录明细新增";
    }

    if (productEvalEntryId > 0) {
        title.current = "试用目录明细编辑";
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
                if (loadProductEvalList) {
                    loadProductEvalList();
                }

                if (loadProductEvalEntryList) {
                    loadProductEvalEntryList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadProductEvalList) {
                    loadProductEvalList();
                }

                if (loadProductEvalEntryList) {
                    loadProductEvalEntryList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/product-eval-entry/detail", {
                        "productEvalEntryId": productEvalEntryId,
                        "productEvalId": productEvalId,
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        let collegeId = undefined;
                        let productId = undefined;

                        if (productEvalEntryId > 0 || productEvalId > 0) {
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
                            beginDate: dayjs(data.beginDate),
                            endDate: dayjs(data.endDate),
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
                        post("/api/product-eval-entry/save", {
                            productEvalEntryId: productEvalEntryId,
                            productEvalId: productEvalId,
                            collegeId: values.collegeId,
                            productId: values.productId,
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

                            if (loadProductEvalEntryList) {
                                loadProductEvalEntryList();
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
                                <DatePicker
                                    style={{width: "100%"}}
                                    placeholder="开始时间"
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
                                <DatePicker
                                    style={{width: "100%"}}
                                    placeholder="结束时间"
                                />
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

class ProductEvalEntryDetail extends React.Component {
    constructor(props) {
        super(props);

        const productEvalId = props.productEvalId;
        this.loadProductEvalList = props.loadProductEvalList;
        this.loadProductEvalEntryList = props.loadProductEvalEntryList;

        this.state = {
            productEvalEntryId: 0,
            productEvalId: productEvalId,
            open: false,
        };
    }

    show = (productEvalEntryId) => {
        this.setState((pre) => {
            return {
                ...pre,
                productEvalEntryId: productEvalEntryId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                productEvalEntryId: 0,
                open: false,
            };
        });
    }

    render() {
        const productEvalEntryId = this.state.productEvalEntryId;
        const productEvalId = this.state.productEvalId;
        const open = this.state.open;
        const loadProductEvalList = this.loadProductEvalList;
        const loadProductEvalEntryList = this.loadProductEvalEntryList;

        return (
            <ProductEvalEntryDetailInner
                productEvalEntryId={productEvalEntryId}
                productEvalId={productEvalId}
                open={open}
                self={this}
                loadProductEvalList={loadProductEvalList}
                loadProductEvalEntryList={loadProductEvalEntryList}
            />
        )
    }
}

export default ProductEvalEntryDetail;

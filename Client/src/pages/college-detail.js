import {Button, ConfigProvider, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";

const CollegeDetailInner = (props) => {
    const collegeId = props.collegeId;
    const open = props.open;
    const self = props.self;
    const loadCollegeList = props.loadCollegeList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (collegeId === 0) {
        title.current = "院校目录新增";
    }

    if (collegeId > 0) {
        title.current = "院校目录编辑";
    }

    const [state, setState] = useState({
        provinceList: [],
        levelList: [],
    });

    const form = useRef();

    return (
        <Modal
            title={title.current}
            width="680px"
            destroyOnClose={true}
            footer={null}
            open={open}
            onOk={() => {
                if (loadCollegeList) {
                    loadCollegeList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadCollegeList) {
                    loadCollegeList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/college/detail", {
                        "id": collegeId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            code: data.code,
                            name: data.name,
                            province: data.province,
                            level: data.level,
                        });

                        setState((pre) => {
                            return {
                                ...pre,
                                provinceList: data.provinceList,
                                levelList: data.levelList,
                            };
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
                        post("/api/college/save", {
                            id: collegeId,
                            code: values.code,
                            name: values.name,
                            province: values.province,
                            level: values.level,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadCollegeList) {
                                loadCollegeList();
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
                        <Descriptions.Item label="院校标识码">
                            <Form.Item
                                name="code"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入院校标识码!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={64}
                                    placeholder="院校标识码"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="院校名称">
                            <Form.Item
                                name="name"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入院校名称!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="院校名称"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="省份">
                            <Form.Item
                                name="province"
                                rules={[
                                    {
                                        required: true,
                                        message: '请选择省份!',
                                    },
                                ]}
                            >
                                <Select
                                    showSearch={true}
                                    optionFilterProp="label"
                                    notFoundContent={"暂无数据, 请输入正确的搜索条件"}
                                    placeholder="请选择省份"
                                    options={state.provinceList}
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="办学层次">
                            <Form.Item
                                name="level"
                                rules={[
                                    {
                                        required: true,
                                        message: '请选择办学层次!',
                                    },
                                ]}
                            >
                                <Select
                                    placeholder="请选择办学层次"
                                    options={state.levelList}
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

class CollegeDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadCollegeList = props.loadCollegeList;

        this.state = {
            collegeId: 0,
            open: false,
        };
    }

    show = (collegeId) => {
        this.setState((pre) => {
            return {
                ...pre,
                collegeId: collegeId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                collegeId: 0,
                open: false,
            };
        });
    }

    render() {
        const collegeId = this.state.collegeId;
        const open = this.state.open;
        const loadCollegeList = this.loadCollegeList;

        return (
            <CollegeDetailInner
                collegeId={collegeId}
                open={open}
                self={this}
                loadCollegeList={loadCollegeList}
            />
        )
    }
}

export default CollegeDetail;

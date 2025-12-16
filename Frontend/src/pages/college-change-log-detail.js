import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const CollegeChangeLogDetailInner = (props) => {
    const collegeChangeLogId = props.collegeChangeLogId;
    const collegeId = props.collegeId;
    const open = props.open;
    const self = props.self;
    const loadCollegeChangeLogList = props.loadCollegeChangeLogList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (collegeChangeLogId === 0) {
        title.current = "院校更新记录新增";
    }

    if (collegeChangeLogId > 0) {
        title.current = "院校更新记录编辑";
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
                if (loadCollegeChangeLogList) {
                    loadCollegeChangeLogList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadCollegeChangeLogList) {
                    loadCollegeChangeLogList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/college-change-log/detail", {
                        "id": collegeChangeLogId,
                        "collegeId": collegeId,
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            collegeId: data.collegeId,
                            changeDesc: data.changeDesc,
                            formerName: data.formerName,
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
                        post("/api/college-change-log/save", {
                            id: collegeChangeLogId,
                            collegeId: collegeId,
                            changeDesc: values.changeDesc,
                            formerName: values.formerName,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadCollegeChangeLogList) {
                                loadCollegeChangeLogList();
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
                        <Descriptions.Item label="曾用名">
                            <Form.Item
                                name="formerName"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入曾用名!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="曾用名"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="更新描述">
                            <Form.Item
                                name="changeDesc"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入更新描述!',
                                    },
                                ]}
                            >
                                <Input.TextArea
                                    rows={3}
                                    maxLength={500}
                                    placeholder="更新描述"
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

class CollegeChangeLogDetail extends React.Component {
    constructor(props) {
        super(props);

        const collegeId = props.collegeId;
        this.loadCollegeChangeLogList = props.loadCollegeChangeLogList;

        this.state = {
            collegeChangeLogId: 0,
            collegeId: collegeId,
            open: false,
        };
    }

    show = (collegeChangeLogId) => {
        this.setState((pre) => {
            return {
                ...pre,
                collegeChangeLogId: collegeChangeLogId,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                collegeChangeLogId: 0,
                open: false,
            };
        });
    }

    render() {
        const collegeChangeLogId = this.state.collegeChangeLogId;
        const collegeId = this.state.collegeId;
        const open = this.state.open;
        const loadCollegeChangeLogList = this.loadCollegeChangeLogList;

        return (
            <CollegeChangeLogDetailInner
                collegeChangeLogId={collegeChangeLogId}
                collegeId={collegeId}
                open={open}
                self={this}
                loadCollegeChangeLogList={loadCollegeChangeLogList}
            />
        )
    }
}

export default CollegeChangeLogDetail;

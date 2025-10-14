import { Button, ConfigProvider, Descriptions, Form, Input, Modal, Select, Space, theme } from "antd";
import React, { useRef, useState } from "react";
import useSiteContext from "../site/site-context";

const PresetQuestionDetailInner = (props) => {
    const questionId = props.questionId;
    const open = props.open;
    const self = props.self;
    const loadQuestionList = props.loadQuestionList;

    const { token } = theme.useToken();
    const { post, showSuccess, showError } = useSiteContext();

    const title = useRef("");
    if (questionId === 0) {
        title.current = "预设问题新增";
    }

    if (questionId > 0) {
        title.current = "预设问题编辑";
    }

    const [state, setState] = useState({
        statusList: [],
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
                if (loadQuestionList) {
                    loadQuestionList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadQuestionList) {
                    loadQuestionList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/admin/preset/question/detail", {
                        "id": questionId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        setState({
                            statusList: data.statusList,
                        });

                        form.current.setFieldsValue({
                            question: data.question,
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
                        post("/api/admin/preset/question/save", {
                            id: questionId,
                            question: values.question,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadQuestionList) {
                                loadQuestionList();;
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
                        labelStyle={{ width: "120px" }}
                    >
                        <Descriptions.Item label="问题">
                            <Form.Item
                                name="question"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入问题!',
                                    },
                                ]}
                            >
                                <Input.TextArea 
                                    rows={4}
                                    maxLength={300}
                                    placeholder="问题"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                    </Descriptions>

                    <Form.Item style={{ marginTop: token.formButtonMarginTop, textAlign: "right" }}>
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

class PresetQuestionDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadQuestionList = props.loadQuestionList;

        this.state = {
            questionId: -1,
            open: false,
        };
    }

    show = (questionId) => {
        this.setState({
            questionId: questionId,
            open: true,
        });
    }

    hide = () => {
        this.setState({
            questionId: -1,
            open: false,
        });
    }

    render() {
        const questionId = this.state.questionId;
        const open = this.state.open;
        const loadQuestionList = this.loadQuestionList;

        return (
            <PresetQuestionDetailInner
                questionId={questionId}
                open={open}
                self={this}
                loadQuestionList={loadQuestionList}
            />
        )
    }
}

export default PresetQuestionDetail;
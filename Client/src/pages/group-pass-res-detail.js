import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const GroupPassResDetailInner = (props) => {
    const groupPassResId = props.groupPassResId;
    const open = props.open;
    const self = props.self;
    const loadGroupPassResList = props.loadGroupPassResList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (groupPassResId === 0) {
        title.current = "群组授权新增";
    }

    if (groupPassResId > 0) {
        title.current = "群组授权编辑";
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
                if (loadGroupPassResList) {
                    loadGroupPassResList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadGroupPassResList) {
                    loadGroupPassResList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/group-pass-res/detail", {
                        "id": groupPassResId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            groupId: data.groupId,
                            passResId: data.passResId,
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
                        post("/api/group-pass-res/save", {
                            id: groupPassResId,
                            groupId: values.groupId,
                            passResId: values.passResId,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadGroupPassResList) {
                                loadGroupPassResList();
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
                        <Descriptions.Item label="群组">
                            <Form.Item
                                name="groupId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入群组!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="群组"
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="资源">
                            <Form.Item
                                name="passResId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入资源!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="资源"
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

class GroupPassResDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadGroupPassResList = props.loadGroupPassResList;

        this.state = {
            groupPassResId: 0,
            open: false,
        };
    }

    show = (groupPassResId) => {
        this.setState((pre) => {
           return {
                ...pre,
                groupPassResId: groupPassResId,
                open: true,
           };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                groupPassResId: 0,
                open: false,
            };
        });
    }

    render() {
        const groupPassResId = this.state.groupPassResId;
        const open = this.state.open;
        const loadGroupPassResList = this.loadGroupPassResList;

        return (
            <GroupPassResDetailInner
                groupPassResId={groupPassResId}
                open={open}
                self={this}
                loadGroupPassResList={loadGroupPassResList}
            />
        )
    }
}

export default GroupPassResDetail;

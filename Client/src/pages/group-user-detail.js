import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const GroupUserDetailInner = (props) => {
    const groupUserId = props.groupUserId;
    const open = props.open;
    const self = props.self;
    const loadGroupUserList = props.loadGroupUserList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (groupUserId === 0) {
        title.current = "群组成员新增";
    }

    if (groupUserId > 0) {
        title.current = "群组成员编辑";
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
                if (loadGroupUserList) {
                    loadGroupUserList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadGroupUserList) {
                    loadGroupUserList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/group-user/detail", {
                        "id": groupUserId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            groupId: data.groupId,
                            userId: data.userId,
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
                        post("/api/group-user/save", {
                            id: groupUserId,
                            groupId: values.groupId,
                            userId: values.userId,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadGroupUserList) {
                                loadGroupUserList();
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
                        <Descriptions.Item label="用户">
                            <Form.Item
                                name="userId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入用户!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder="用户"
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

class GroupUserDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadGroupUserList = props.loadGroupUserList;

        this.state = {
            groupUserId: 0,
            open: false,
        };
    }

    show = (groupUserId) => {
        this.setState((pre) => {
           return {
                ...pre,
                groupUserId: groupUserId,
                open: true,
           };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                groupUserId: 0,
                open: false,
            };
        });
    }

    render() {
        const groupUserId = this.state.groupUserId;
        const open = this.state.open;
        const loadGroupUserList = this.loadGroupUserList;

        return (
            <GroupUserDetailInner
                groupUserId={groupUserId}
                open={open}
                self={this}
                loadGroupUserList={loadGroupUserList}
            />
        )
    }
}

export default GroupUserDetail;

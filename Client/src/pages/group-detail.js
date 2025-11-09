import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const GroupDetailInner = (props) => {
    const groupId = props.groupId;
    const open = props.open;
    const self = props.self;
    const loadGroupList = props.loadGroupList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (groupId === 0) {
        title.current = "群组新增";
    }

    if (groupId > 0) {
        title.current = "群组编辑";
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
                if (loadGroupList) {
                    loadGroupList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadGroupList) {
                    loadGroupList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/group/detail", {
                        "id": groupId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            name: data.name,
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
                        post("/api/group/save", {
                            id: groupId,
                            name: values.name,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadGroupList) {
                                loadGroupList();
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
                        <Descriptions.Item label="群组名称">
                            <Form.Item
                                name="name"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入群组名称!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={128}
                                    placeholder="群组名称"
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

class GroupDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadGroupList = props.loadGroupList;

        this.state = {
            groupId: 0,
            open: false,
        };
    }

    show = (groupId) => {
        this.setState((pre) => {
           return {
                ...pre,
                groupId: groupId,
                open: true,
           };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                groupId: 0,
                open: false,
            };
        });
    }

    render() {
        const groupId = this.state.groupId;
        const open = this.state.open;
        const loadGroupList = this.loadGroupList;

        return (
            <GroupDetailInner
                groupId={groupId}
                open={open}
                self={this}
                loadGroupList={loadGroupList}
            />
        )
    }
}

export default GroupDetail;

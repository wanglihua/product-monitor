import {Button, ConfigProvider, Descriptions, Form, Modal, Select, Space, theme} from "antd";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";

const GroupUserDetailInner = (props) => {
    const groupId = props.groupId;
    const open = props.open;
    const self = props.self;
    const loadGroupUserList = props.loadGroupUserList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const [state, setState] = useState({});

    const [userSelectOptions, setUserSelectOptions] = useState([]);

    const form = useRef();

    return (
        <Modal
            title="群组成员添加"
            width="560px"
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
                        "groupId": groupId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            // userId: data.userId,
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
                            groupId: groupId,
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
                        <Descriptions.Item label="用户">
                            <Form.Item
                                name="userId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请选择用户!',
                                    },
                                ]}
                            >
                                <Select
                                    showSearch={true}
                                    filterOption={false}
                                    notFoundContent={"暂无数据, 请输入正确的搜索条件"}
                                    placeholder="请输入搜索条件, 并选择用户"
                                    options={userSelectOptions}
                                    onSearch={(search) => {
                                        post("/api/user/select/data", {
                                            search: search,
                                        }, function (response) {
                                            const res = response.data;
                                            if (res.success !== true) {
                                                showError(res.message);
                                                return;
                                            }

                                            setUserSelectOptions(res.userList);
                                        });
                                    }}
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

        this.groupId = props.groupId;
        this.loadGroupUserList = props.loadGroupUserList;

        this.state = {
            open: false,
        };
    }

    show = (groupId) => {
        this.setState((pre) => {
            return {
                ...pre,
                open: true,
            };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                open: false,
            };
        });
    }

    render() {
        const open = this.state.open;
        const groupId = this.groupId;
        const loadGroupUserList = this.loadGroupUserList;

        return (
            <GroupUserDetailInner
                groupId={groupId}
                open={open}
                self={this}
                loadGroupUserList={loadGroupUserList}
            />
        )
    }
}

export default GroupUserDetail;

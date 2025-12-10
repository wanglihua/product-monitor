import {Button, ConfigProvider, DatePicker, Descriptions, Form, Input, Modal, Select, Space, theme} from "antd";
import {CloseCircleOutlined, SaveOutlined} from "@ant-design/icons";
import React, {useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import dayjs from "dayjs";

const UserProvinceDetailInner = (props) => {
    const userProvinceId = props.userProvinceId;
    const open = props.open;
    const self = props.self;
    const loadUserProvinceList = props.loadUserProvinceList;

    const {token} = theme.useToken();
    const {post, showSuccess, showError} = useSiteContext();

    const title = useRef("");
    if (userProvinceId === 0) {
        title.current = "新增";
    }

    if (userProvinceId > 0) {
        title.current = "编辑";
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
                if (loadUserProvinceList) {
                    loadUserProvinceList();
                }

                self.hide();
            }}
            onCancel={() => {
                if (loadUserProvinceList) {
                    loadUserProvinceList();
                }

                self.hide();
            }}
            afterOpenChange={(open) => {
                if (open === true) {
                    post("/api/user-province/detail", {
                        "id": userProvinceId
                    }, function (response) {
                        const res = response.data;
                        if (res.success !== true) {
                            showError(res.message);
                            return;
                        }

                        const data = res.data;

                        form.current.setFieldsValue({
                            userId: data.userId,
                            provinceType: data.provinceType,
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
                        post("/api/user-province/save", {
                            id: userProvinceId,
                            userId: values.userId,
                            provinceType: values.provinceType,
                        }, function (response) {
                            const res = response.data;
                            if (res.success === false) {
                                showError(res.message);
                                return;
                            }

                            showSuccess(res.message);

                            if (loadUserProvinceList) {
                                loadUserProvinceList();
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
                        <Descriptions.Item label="">
                            <Form.Item
                                name="userId"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={8}
                                    placeholder=""
                                    allowClear
                                />
                            </Form.Item>
                        </Descriptions.Item>
                        <Descriptions.Item label="">
                            <Form.Item
                                name="provinceType"
                                rules={[
                                    {
                                        required: true,
                                        message: '请输入!',
                                    },
                                ]}
                            >
                                <Input
                                    maxLength={4}
                                    placeholder=""
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

class UserProvinceDetail extends React.Component {
    constructor(props) {
        super(props);

        this.loadUserProvinceList = props.loadUserProvinceList;

        this.state = {
            userProvinceId: 0,
            open: false,
        };
    }

    show = (userProvinceId) => {
        this.setState((pre) => {
           return {
                ...pre,
                userProvinceId: userProvinceId,
                open: true,
           };
        });
    }

    hide = () => {
        this.setState((pre) => {
            return {
                ...pre,
                userProvinceId: 0,
                open: false,
            };
        });
    }

    render() {
        const userProvinceId = this.state.userProvinceId;
        const open = this.state.open;
        const loadUserProvinceList = this.loadUserProvinceList;

        return (
            <UserProvinceDetailInner
                userProvinceId={userProvinceId}
                open={open}
                self={this}
                loadUserProvinceList={loadUserProvinceList}
            />
        )
    }
}

export default UserProvinceDetail;

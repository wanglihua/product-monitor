import {Button, ConfigProvider, Descriptions, Form, Input, Space, theme} from 'antd';
import {useRef} from "react";
import {SaveOutlined} from '@ant-design/icons';
import useSiteContext from "../site/site-context";
import Title from "../site/title";

const UserPassword = () => {

    const {token} = theme.useToken();
    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const form = useRef();

    return (
        <div>
            <Title title="修改密码" breadcrumb={[
                {title: "修改密码"},
            ]}/>

            <div style={{width: "460px"}}>
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
                        }}
                    >
                        <Descriptions
                            bordered={true}
                            layout="horizontal"
                            size="small"
                            column={1}
                            labelStyle={{width: "120px"}}
                        >
                            <Descriptions.Item label="原密码">
                                <Form.Item
                                    name="oldPassword"
                                    rules={[
                                        {
                                            required: true,
                                            message: '请输入原密码!',
                                        },
                                    ]}
                                >
                                    <Input.Password
                                        maxLength={18}
                                        placeholder="原密码"
                                        allowClear
                                    />
                                </Form.Item>
                            </Descriptions.Item>
                            <Descriptions.Item label="新密码">
                                <Form.Item
                                    name="newPassword"
                                    rules={[
                                        {
                                            required: true,
                                            message: '请输入新密码!',
                                        },
                                    ]}
                                >
                                    <Input.Password
                                        maxLength={18}
                                        placeholder="新密码"
                                        allowClear
                                    />
                                </Form.Item>
                            </Descriptions.Item>
                            <Descriptions.Item label="新密码重输">
                                <Form.Item
                                    name="newPasswordAgain"
                                    rules={[
                                        {
                                            required: true,
                                            message: '请再次输入新密码!',
                                        },
                                    ]}
                                >
                                    <Input.Password
                                        maxLength={18}
                                        placeholder="新密码重输"
                                        allowClear
                                    />
                                </Form.Item>
                            </Descriptions.Item>
                        </Descriptions>

                        <Form.Item style={{marginTop: token.formButtonMarginTop, textAlign: "right"}}>
                            <Space>
                                <Button type="primary" size="large" htmlType="submit">
                                    <SaveOutlined/>&nbsp;保存
                                </Button>
                            </Space>
                        </Form.Item>
                    </Form>
                </ConfigProvider>
            </div>
        </div>
    );
}

export default UserPassword;
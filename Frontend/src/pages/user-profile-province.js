import {Button, ConfigProvider, Descriptions, Form, Input, Space, theme} from 'antd';
import {useRef} from "react";
import {SaveOutlined} from '@ant-design/icons';
import useSiteContext from "../site/site-context";
import Title from "../site/title";

const UserProfileProvince = () => {

    return (
        <div>
            <Title title="负责区域" breadcrumb={[
                {title: "负责区域"},
            ]}/>

            <div style={{width: "460px"}}>
                <Descriptions
                    bordered={true}
                    layout="horizontal"
                    size="small"
                    column={1}
                >
                    <Descriptions.Item label="">
                        <span>Hello</span>
                    </Descriptions.Item>
                </Descriptions>
            </div>
        </div>
    );
};

export default UserProfileProvince;
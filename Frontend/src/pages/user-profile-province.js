import {Button, ConfigProvider, Descriptions, Form, Input, Space, theme} from 'antd';
import {useEffect, useRef, useState} from "react";
import useSiteContext from "../site/site-context";
import Title from "../site/title";

const UserProfileProvince = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [state, setState] = useState({
        provinceList: [],
    });

    useEffect(() => {
        post("/api/user/profile/province", {}, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    provinceList: res.provinceList,
                };
            });
        });
    }, []);

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
                    {state.provinceList && state.provinceList.map((item, index) => (
                        <Descriptions.Item label={"区域" + (index + 1)}>
                            <span>{{item}}</span>
                        </Descriptions.Item>
                    ))}
                </Descriptions>
            </div>
        </div>
    );
};

export default UserProfileProvince;
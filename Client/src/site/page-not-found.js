import { Button, Result } from 'antd';
import React from 'react';
import { useNavigate } from 'react-router';
import { HomeOutlined } from '@ant-design/icons';

const PageNotFound = () => {
    const navigate = useNavigate();

    return (
        <Result
            status="404"
            title="404"
            subTitle="sorry, 您访问的页面不存在!"
            extra={
                <Button type="primary" size="large" onClick={(e) => {
                    navigate("/");
                }}>
                    <HomeOutlined />返回首页
                </Button >
            }
        />
    );
};

export default PageNotFound;

import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Select, Space, Spin, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import ProductUrlDetail from "./product-url-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const Test = () => {
    const {post, showSuccess, showError} = useSiteContext();

    const [options, setOptions] = useState([]);

    return (
        <div>
            <Title title="测试" menuKey="test" breadcrumb={[
                {title: "测试"},
            ]}/>

            <Space>
                <Select
                    showSearch={true}
                    options={options}
                    filterOption={false}
                    placeholder="请搜索选择学校"
                    notFoundContent={"暂无数据"}
                    style={{
                        width: "280px",
                    }}
                    onSearch={(search) => {
                        post("/api/college/select/data", {
                            search: search,
                        }, function (response) {
                            const res = response.data;
                            if (res.success !== true) {
                                showError(res.message);
                                return;
                            }

                            setOptions(res.collegeList);
                        });
                    }}
                >

                < /Select>
            </Space>
        </div>
    );
};

export default Test;
import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const AuthResult = () => {

    const {post, showError} = useSiteContext();

    const [listParam, setListParam] = useState({
        nameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        authResultList: [],
        authResultCount: 0,
    });

    const loadAuthResultList = () => {
        post("/api/auth-result/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    authResultList: res.data.authResultList,
                    authResultCount: res.data.authResultCount,
                };
            });
        });
    };

    const authResultListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "资源类别名称",
            dataIndex: "passResCateName",
            width: "130px",
        },
        {
            title: "资源名称",
            dataIndex: "passResName",
            width: "120px",
        },
        {
            title: "登录名",
            dataIndex: "loginName",
            width: "120px",
        },
        {
            title: "用户名",
            dataIndex: "userName",
            width: "120px",
        },
        {
            title: "群组名称",
            dataIndex: "groupName",
            width: "150px",
        },
        {
            title: "授权时间",
            dataIndex: "createTime",
            width: "150px",
        },
        {
            title: "",
        },
    ];

    useEffect(() => {
        loadAuthResultList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();

    return (
        <div>
            <Title title="" menuKey="auth-result" breadcrumb={[
                {title: ""},
            ]}/>
            <div>
                <Space>
                    <Input ref={nameQueryRef} addonBefore="名称" placeholder="名称查询" style={{width: 320}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const nameQuery = nameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    nameQuery: nameQuery,
                                    current_millis: Date.now(),
                                };
                            });
                        }}
                    >
                        查询
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={authResultListColumns} rowKey="id"
                       dataSource={state.authResultList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.authResultCount,
                           showQuickJumper: true,
                           showSizeChanger: true,
                           showTotal: (total, range) => `${range[0]}-${range[1]} of ${total}`,
                       }}
                       onChange={(pagination, filters, sorter, extra) => {
                           setListParam((pre) => {
                               return {
                                   ...pre,
                                   currentPage: pagination.current,
                                   pageSize: pagination.pageSize,
                                   sortField: sorter.field,
                                   sortOrder: sorter.order,
                               };
                           });
                       }}
                />
            </div>
        </div>
    );
};

export default AuthResult;

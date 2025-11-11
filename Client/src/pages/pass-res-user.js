import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import {useParams} from "react-router";
import useSiteContext from "../site/site-context";
import Title from "../site/title";

const PassResUser = () => {

    const params = useParams();
    const passResId = params.passResId;

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        nameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        userPassResList: [],
        userPassResCount: 0,
    });

    const loadUserPassResList = () => {
        post("/api/user-pass-res/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    userPassResList: res.data.userPassResList,
                    userPassResCount: res.data.userPassResCount,
                };
            });
        });
    };

    const userPassResListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "用户",
            dataIndex: "userId",
            width: "200px",
        },
        {
            title: "授权资源",
            dataIndex: "passResId",
            width: "200px",
        },
        {
            title: "创建时间",
            dataIndex: "createTime",
            width: "150px",
            sorter: true,
        },
        {
            title: "更新时间",
            dataIndex: "updateTime",
            width: "150px",
            sorter: true,
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const userPassResId = record.id;
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/user-pass-res/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadUserPassResList();
                                });
                            });
                        }}>
                            <DeleteOutlined/>删除
                        </Button>
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadUserPassResList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();

    return (
        <div>
            <Title title="用户授权" menuKey="user-pass-res" breadcrumb={[
                {title: "用户授权"},
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
                    <Button
                        color="default"
                        icon={<PlusCircleOutlined style={{color: "green"}}/>}
                        onClick={(e) => {
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={userPassResListColumns} rowKey="id"
                       dataSource={state.userPassResList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.userPassResCount,
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

export default PassResUser;

import React, {useEffect, useRef, useState} from 'react';
import {Badge, Button, Input, Space, Table} from 'antd';
import {DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import {useParams, useNavigate} from "react-router";
import useSiteContext from "../site/site-context";
import Title from "../site/title";

const UserPassRes = () => {

    const params = useParams();
    const userId = params.userId;

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        userId: userId,
        passResNameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        userPassResList: [],
        userPassResCount: 0,
        loginName: "",
        userName: "",
    });

    const loadUserPassResList = () => {
        post("/api/user-pass-res/pass/res/list", listParam, function (response) {
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
                    loginName: res.data.loginName,
                    userName: res.data.userName,
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
            title: "资源类别",
            dataIndex: "passResCateName",
            width: "150px",
        },
        {
            title: "授权资源",
            dataIndex: "passResName",
            width: "200px",
        },
        {
            title: "是否授权",
            dataIndex: "grant",
            width: "150px",
            render: (value, record, index) => {
                if (value === true) {
                    return <span><Badge status="success"/>&nbsp;已授权</span>;
                }

                if (value === false) {
                    return <span><Badge status="error"/>&nbsp;未授权</span>;
                }

                return <span></span>;
            }
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        {record.grant === true && (
                            <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                                post("/api/user-pass-res/revoke", {
                                    userId: userId,
                                    passResId: record.passResId,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadUserPassResList();
                                });
                            }}>
                                <DeleteOutlined/>收权
                            </Button>
                        )}

                        {record.grant === false && (
                            <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                                post("/api/user-pass-res/grant", {
                                    userId: userId,
                                    passResId: record.passResId,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadUserPassResList();
                                });
                            }}>
                                <PlusCircleOutlined/>授权
                            </Button>
                        )}
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadUserPassResList();
    }, [JSON.stringify(listParam)]);

    const passResNameQueryRef = useRef();

    return (
        <div>
            <Title title={`用户权限资源 ${state.loginName} ${state.userName}`} parentMenuKeys={["system-manage"]} menuKey="user" breadcrumb={[
                {title: "系统管理"},
                {
                    title: <span style={{cursor: "pointer"}} onClick={() => {
                        navigate("/user");
                    }}>用户</span>
                },
                {title: "用户权限资源"},
            ]}/>
            <div>
                <Space>
                    <Input ref={passResNameQueryRef} addonBefore="资源名称" placeholder="资源名称查询" style={{width: 320}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const passResNameQuery = passResNameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    passResNameQuery: passResNameQuery,
                                    current_millis: Date.now(),
                                };
                            });
                        }}
                    >
                        查询
                    </Button>
                </Space>
            </div>
            <div style={{padding: "35px 0"}}>
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

export default UserPassRes;

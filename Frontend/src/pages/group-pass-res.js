import React, {useEffect, useRef, useState} from 'react';
import {Badge, Button, Input, Space, Table} from 'antd';
import {DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import {useParams, useNavigate} from "react-router";
import useSiteContext from "../site/site-context";
import Title from "../site/title";

const GroupPassRes = () => {

    const params = useParams();
    const groupId = params.groupId;

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        groupId: groupId,
        passResNameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        groupPassResList: [],
        groupPassResCount: 0,
        groupName: "",
    });

    const loadGroupPassResList = () => {
        post("/api/group-pass-res/pass/res/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    groupPassResList: res.data.groupPassResList,
                    groupPassResCount: res.data.groupPassResCount,
                    groupName: res.data.groupName,
                };
            });
        });
    };

    const groupPassResListColumns = [
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
                                post("/api/group-pass-res/revoke", {
                                    groupId: groupId,
                                    passResId: record.passResId,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadGroupPassResList();
                                });
                            }}>
                                <DeleteOutlined/>收权
                            </Button>
                        )}

                        {record.grant === false && (
                            <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                                post("/api/group-pass-res/grant", {
                                    groupId: groupId,
                                    passResId: record.passResId,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadGroupPassResList();
                                });
                            }}>
                                <PlusCircleOutlined/>授权
                            </Button>
                        )}
                    </Space>
                )
                    ;
            },
        },
    ];

    useEffect(() => {
        loadGroupPassResList();
    }, [JSON.stringify(listParam)]);

    const passResNameQueryRef = useRef();

    return (
        <div>
            <Title title={`群组权限资源 ${state.groupName}`} parentMenuKeys={["system-manage"]} menuKey="group" breadcrumb={[
                {title: "系统管理"},
                {
                    title: <span style={{cursor: "pointer"}} onClick={() => {
                        navigate("/group");
                    }}>群组</span>
                },
                {title: "群组权限资源"},
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
            <div style={{padding: "35px 0"}}>
                <Table columns={groupPassResListColumns} rowKey="id"
                       dataSource={state.groupPassResList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.groupPassResCount,
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

export default GroupPassRes;

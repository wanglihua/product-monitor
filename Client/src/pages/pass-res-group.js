import React, {useEffect, useRef, useState} from 'react';
import {Badge, Button, Input, Space, Table} from 'antd';
import {DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import {useParams, useNavigate} from "react-router";
import useSiteContext from "../site/site-context";
import Title from "../site/title";

const PassResGroup = () => {

    const params = useParams();
    const passResId = params.passResId;

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        passResId: passResId,
        groupNameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        passResGroupList: [],
        passResGroupCount: 0,
        passResName: "",
    });

    const loadGroupPassResList = () => {
        post("/api/group-pass-res/group/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    passResGroupList: res.data.passResGroupList,
                    passResGroupCount: res.data.passResGroupCount,
                    passResName: res.data.passResName,
                };
            });
        });
    };

    const passResGroupListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "群组",
            dataIndex: "groupName",
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
                                    groupId: record.groupId,
                                    passResId: passResId,
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
                                    groupId: record.groupId,
                                    passResId: passResId,
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
                );
            },
        },
    ];

    useEffect(() => {
        loadGroupPassResList();
    }, [JSON.stringify(listParam)]);

    const groupNameQueryRef = useRef();

    return (
        <div>
            <Title title={`${state.passResName} 群组授权`} parentMenuKeys={["system-manage"]} menuKey="pass-res" breadcrumb={[
                {title: "系统管理"},
                {
                    title: <span style={{cursor: "pointer"}} onClick={() => {
                        navigate("/pass-res");
                    }}>授权资源</span>
                },
                {title: "群组授权"},
            ]}/>
            <div>
                <Space>
                    <Input ref={groupNameQueryRef} addonBefore="群组名称" placeholder="群组名称查询" style={{width: 320}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const groupNameQuery = groupNameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    groupNameQuery: groupNameQuery,
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
                <Table columns={passResGroupListColumns} rowKey="id"
                       dataSource={state.passResGroupList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.passResGroupCount,
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

export default PassResGroup;

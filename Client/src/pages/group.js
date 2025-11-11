import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {useNavigate} from "react-router";
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined, UnorderedListOutlined} from '@ant-design/icons';
import GroupDetail from "./group-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const Group = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        nameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        groupList: [],
        groupCount: 0,
    });

    const loadGroupList = () => {
        post("/api/group/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    groupList: res.data.groupList,
                    groupCount: res.data.groupCount,
                };
            });
        });
    };

    const groupDetail = useRef();

    const groupListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "群组名称",
            dataIndex: "name",
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
                            const groupId = record.id;
                            navigate(`/group-pass-res/${groupId}`);
                        }}>
                            <i className="iconfont icon-pass-res"/>授权资源
                        </Button>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const groupId = record.id;
                            navigate(`/group-user/${groupId}`);
                        }}>
                            <UnorderedListOutlined/>群组成员
                        </Button>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const groupId = record.id;
                            groupDetail.current.show(groupId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/group/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadGroupList();
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
        loadGroupList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();

    return (
        <div>
            <Title title="群组" parentMenuKeys={["system-manage"]} menuKey="group" breadcrumb={[
                {title: "系统管理"},
                {title: "群组"},
            ]}/>
            <div>
                <Space>
                    <Input ref={nameQueryRef} addonBefore="群组名称" placeholder="群组名称查询" style={{width: 320}}/>
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
                            groupDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={groupListColumns} rowKey="id"
                       dataSource={state.groupList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.groupCount,
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
            <GroupDetail ref={groupDetail} loadGroupList={loadGroupList}/>
        </div>
    );
};

export default Group;

import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import UserProvinceDetail from "./user-province-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const UserProvince = () => {

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
        userProvinceList: [],
        userProvinceCount: 0,
    });

    const loadUserProvinceList = () => {
        post("/api/user-province/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    userProvinceList: res.data.userProvinceList,
                    userProvinceCount: res.data.userProvinceCount,
                };
            });
        });
    };

    const userProvinceDetail = useRef();

    const userProvinceListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "登录名",
            dataIndex: "loginName",
            width: "200px",
        },
        {
            title: "用户名",
            dataIndex: "userName",
            width: "200px",
        },
        {
            title: "省份",
            dataIndex: "provinceType",
            width: "200px",
        },
        {
            title: "添加时间",
            dataIndex: "createTime",
            width: "150px",
            sorter: true,
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const userProvinceId = record.id;
                            userProvinceDetail.current.show(userProvinceId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要移除吗！", "确认", () => {
                                post("/api/user-province/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadUserProvinceList();
                                });
                            });
                        }}>
                            <DeleteOutlined/>移除
                        </Button>
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadUserProvinceList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();

    return (
        <div>
            <Title title="" menuKey="user-province" breadcrumb={[
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
                    <Button
                        color="default"
                        icon={<PlusCircleOutlined style={{color: "green"}}/>}
                        onClick={(e) => {
                            userProvinceDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={userProvinceListColumns} rowKey="id"
                       dataSource={state.userProvinceList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.userProvinceCount,
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
            <UserProvinceDetail ref={userProvinceDetail} loadUserProvinceList={loadUserProvinceList}/>
        </div>
    );
};

export default UserProvince;

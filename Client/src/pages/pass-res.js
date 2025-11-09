import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Select, Space, Table} from 'antd';
import {PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const PassRes = () => {

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
        passResList: [],
        passResCount: 0,
        passResCateList: [],
    });

    const loadPassResList = () => {
        post("/api/pass-res/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    passResList: res.data.passResList,
                    passResCount: res.data.passResCount,
                    passResCateList: res.data.passResCateList,
                };
            });
        });
    };

    const passResListColumns = [
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
            dataIndex: "cateName",
            width: "120px",
        },
        {
            title: "资源名称",
            dataIndex: "name",
            width: "200px",
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                        }}>
                            <i className="iconfont icon-user"/>用户授权
                        </Button>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                        }}>
                            <i className="iconfont icon-group"/>群组授权
                        </Button>
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadPassResList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();
    const cateIdQueryRef = useRef();

    return (
        <div>
            <Title title="授权资源" parentMenuKeys={["system-manage"]} menuKey="pass-res" breadcrumb={[
                {title: "系统管理"},
                {title: "授权码资源"},
            ]}/>
            <div>
                <Space>
                    <Input ref={nameQueryRef} addonBefore="名称" placeholder="名称查询" style={{width: 320}}/>
                    <Select options={state.passResCateList} placeholder="类别查询" style={{width: 130}} onSelect={(val) => {
                        cateIdQueryRef.current = val;
                    }}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const nameQuery = nameQueryRef.current.input.value;
                            const cateIdQuery = cateIdQueryRef.current;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    nameQuery: nameQuery,
                                    cateIdQuery: cateIdQuery,
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
                            showConfirm("确认要初始化最新授权资源数据吗！", "确认", () => {
                                post("/api/pass-res/init/data", {}, function (response) {
                                    const res = response.data;
                                    if (res.success === false) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);

                                    loadPassResList();
                                });
                            });
                        }}
                    >
                        初始化最新数据
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={passResListColumns} rowKey="id"
                       dataSource={state.passResList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.passResCount,
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

export default PassRes;

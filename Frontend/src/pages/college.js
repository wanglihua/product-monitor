import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined, FileExcelOutlined} from '@ant-design/icons';
import CollegeDetail from "./college-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";
import $ from "jquery";
import download from "../core/download";

const College = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        codeQuery: "",
        nameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        collegeList: [],
        collegeCount: 0,
    });

    const loadCollegeList = () => {
        post("/api/college/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    collegeList: res.data.collegeList,
                    collegeCount: res.data.collegeCount,
                };
            });
        });
    };

    const collegeDetail = useRef();

    const collegeListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "院校标识码",
            dataIndex: "code",
            width: "120px",
        },
        {
            title: "院校名称",
            dataIndex: "name",
            width: "200px",
        },
        {
            title: "省份",
            dataIndex: "province",
            width: "150px",
        },
        {
            title: "办学层次",
            dataIndex: "level",
            width: "100px",
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
                            const collegeId = record.id;
                            collegeDetail.current.show(collegeId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/college/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadCollegeList();
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
        loadCollegeList();
    }, [JSON.stringify(listParam)]);

    const codeQueryRef = useRef();
    const nameQueryRef = useRef();

    return (
        <div>
            <Title title="院校目录" parentMenuKeys={["basic-setting"]} menuKey="college" breadcrumb={[
                {title: "基础设置"},
                {title: "院校目录"},
            ]}/>
            <div>
                <Space>
                    <Input ref={codeQueryRef} addonBefore="标识" placeholder="标识查询" style={{width: 180}}/>
                    <Input ref={nameQueryRef} addonBefore="名称" placeholder="名称查询" style={{width: 280}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const codeQuery = codeQueryRef.current.input.value;
                            const nameQuery = nameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    codeQuery: codeQuery,
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
                            collegeDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                    <Button
                        color="default"
                        icon={<FileExcelOutlined/>}
                        onClick={(e) => {
                            showConfirm("确认要导出吗！", "确认", () => {
                                const query = $.param(listParam);
                                const url = `/api/college/export?${query}`;
                                download(url);

                                showSuccess("导出成功, 文件下载中!");
                            });
                        }}
                    >
                        导出
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={collegeListColumns} rowKey="id"
                       dataSource={state.collegeList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.collegeCount,
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
            <CollegeDetail ref={collegeDetail} loadCollegeList={loadCollegeList}/>
        </div>
    );
};

export default College;

import React, { useEffect, useRef, useState } from 'react';
import { Button, Input, Space, Table } from 'antd';
import { EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined } from '@ant-design/icons';
import ProductDetail from "./product-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const Product = () => {

    const { post, showConfirm, showError, showSuccess } = useSiteContext();

    const [listParam, setListParam] = useState({
        questionQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
    });

    const [state, setState] = useState({
        questionList: [],
        questionCount: 0,
    });

    const loadProductList = () => {
        post("/api/admin/preset/question/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    questionList: res.data.questionList,
                    questionCount: res.data.questionCount,
                };
            });
        });
    };

    const questionDetail = useRef();

    const questionListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "问题",
            dataIndex: "question",
            width: "580px",
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
            align: "center",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{ fontSize: "12px" }} onClick={(e) => {
                            const questionId = record.id;
                            questionDetail.current.show(questionId);
                        }}>
                            <EditOutlined />编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{ fontSize: "12px" }} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/admin/preset/question/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductList();
                                });
                            });
                        }}>
                            <DeleteOutlined />删除
                        </Button>
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadProductList();
    }, [JSON.stringify(listParam)]);

    const questionQueryRef = useRef();

    return (
        <div>
            <Title title="预设问题" menuKey="preset-question" breadcrumb={[
                { title: "预设问题" },
            ]} />
            <Space direction="vertical" size="large">
                <div>
                    <Space>
                        <Input ref={questionQueryRef} addonBefore="问题" placeholder="问题查询" style={{ width: 320 }} />
                        <Button
                            color="default"
                            icon={<SearchOutlined />}
                            onClick={(e) => {
                                const questionQuery = questionQueryRef.current.input.value;

                                setListParam((pre) => {
                                    return {
                                        ...pre,
                                        questionQuery: questionQuery,
                                    };
                                });
                            }}
                        >
                            查询
                        </Button>
                        <Button
                            color="default"
                            icon={<PlusCircleOutlined style={{ color: "green" }} />}
                            onClick={(e) => {
                                questionDetail.current.show(0);
                            }}
                        >
                            添加
                        </Button>
                    </Space>
                </div>

                <div>
                    <Table columns={questionListColumns} rowKey="id"
                        dataSource={state.questionList}
                        pagination={{
                            current: listParam.currentPage,
                            pageSize: listParam.pageSize,
                            total: state.userCount,
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
            </Space>
            <ProductDetail ref={questionDetail} loadProductList={loadProductList} />
        </div>
    );
};

export default Product;
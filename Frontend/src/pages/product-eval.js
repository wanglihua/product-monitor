import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table, Upload} from 'antd';
import {DeleteOutlined, FileExcelOutlined, PlusCircleOutlined, SearchOutlined, UnorderedListOutlined} from '@ant-design/icons';
import {useNavigate} from "react-router";
import ProductEvalEntryDetail from "./product-eval-entry-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";
import download from "../core/download";

const ProductEval = () => {

    const {post, showConfirm, showError, showSuccess, showMask, hideMask} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        collegeNameQuery: "",
        productNameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        productEvalList: [],
        productEvalCount: 0,
    });

    const loadProductEvalList = () => {
        post("/api/product-eval/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productEvalList: res.data.productEvalList,
                    productEvalCount: res.data.productEvalCount,
                };
            });
        });
    };

    const productEvalEntryDetail = useRef();

    const productEvalListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "学校",
            dataIndex: "collegeName",
            width: "200px",
        },
        {
            title: "产品",
            dataIndex: "productName",
            width: "250px",
        },
        {
            title: "试用次数",
            align: "center",
            dataIndex: "evalTimes",
            width: "90px",
        },
        {
            title: "售后次数",
            align: "center",
            dataIndex: "serviceTimes",
            width: "90px",
        },
        {
            title: "累计天数",
            align: "center",
            dataIndex: "totalDays",
            width: "90px",
        },
        {
            title: "剩余天数",
            align: "center",
            dataIndex: "remainDays",
            width: "90px",
        },
        {
            title: "开始时间",
            dataIndex: "beginDate",
            width: "110px",
        },
        {
            title: "结束时间",
            dataIndex: "endDate",
            width: "110px",
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const productEvalId = record.id;
                            navigate(`/product-eval-entry/${productEvalId}`);
                        }}>
                            <UnorderedListOutlined/>试用记录
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("此操作将同时删除关联明细记录，确认要删除吗！", "确认", () => {
                                post("/api/product-eval/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);

                                    loadProductEvalList();
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
        loadProductEvalList();
    }, [JSON.stringify(listParam)]);

    const collegeNameQueryRef = useRef();
    const productNameQueryRef = useRef();

    return (
        <div>
            <Title title="试用目录" parentMenuKeys={["project-manage"]} menuKey="product-eval" breadcrumb={[
                {title: "项目管理"},
                {title: "试用目录"},
            ]}/>
            <div>
                <Space>
                    <Input ref={collegeNameQueryRef} addonBefore="学校名称" placeholder="学校名称查询" style={{width: 280}}/>
                    <Input ref={productNameQueryRef} addonBefore="产品名称" placeholder="产品名称查询" style={{width: 280}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const collegeNameQuery = collegeNameQueryRef.current.input.value;
                            const productNameQuery = productNameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    collegeNameQuery: collegeNameQuery,
                                    productNameQuery: productNameQuery,
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
                            productEvalEntryDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                    <Upload
                        showUploadList={false}
                        name="excelFile"
                        accept=".xlsx"
                        action="/api/product-eval/import"
                        onChange={(info) => {
                            const status = info.file.status;

                            if (status === 'uploading') {
                                showMask();
                            }

                            if (status === 'done') {
                                hideMask();
                                const response = info.file.response;
                                if (response.success !== true) {
                                    showError(response.message);
                                    return;
                                }

                                showSuccess(response.message);
                                loadProductEvalList();
                            }

                            if (status === 'error') {
                                hideMask();
                                showError("文件上传失败, 请重试!");
                            }
                        }}
                    >
                        <Button
                            color="default"
                            icon={<FileExcelOutlined/>}
                        >
                            导入
                        </Button>
                    </Upload>
                    <a onClick={() => {
                        const url = "/api/product-eval/export";
                        download(url);
                    }}>下载导入样板文件</a>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productEvalListColumns} rowKey="id"
                       dataSource={state.productEvalList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productEvalCount,
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
            <ProductEvalEntryDetail ref={productEvalEntryDetail} loadProductEvalList={loadProductEvalList}/>
        </div>
    );
};

export default ProductEval;

import React, {useEffect, useRef, useState} from 'react';
import {Button, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined} from '@ant-design/icons';
import {useParams, useNavigate} from "react-router";
import ProductEvalEntryDetail from "./product-eval-entry-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductEvalEntry = () => {

    const params = useParams();
    const productEvalId = params.productEvalId;

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        productEvalId: productEvalId,
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        productEvalEntryList: [],
        productEvalEntryCount: 0,
        collegeName: "",
        productName: "",
    });

    const loadProductEvalEntryList = () => {
        post("/api/product-eval-entry/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productEvalEntryList: res.data.productEvalEntryList,
                    productEvalEntryCount: res.data.productEvalEntryCount,
                    collegeName: res.data.collegeName,
                    productName: res.data.productName,
                };
            });
        });
    };

    const productEvalEntryDetail = useRef();

    const productEvalEntryListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "累计天数",
            dataIndex: "totalDays",
            width: "110px",
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
            title: "记录人",
            dataIndex: "userName",
            width: "150px",
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
                            const productEvalEntryId = record.id;
                            productEvalEntryDetail.current.show(productEvalEntryId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-eval-entry/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);

                                    if (res.data && res.data.productEvalDeleted === true) {
                                        navigate("/product-eval");
                                        return;
                                    }

                                    loadProductEvalEntryList();
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
        loadProductEvalEntryList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();

    return (
        <div>
            <Title title="试用目录明细" parentMenuKeys={["project-manage"]} menuKey="product-eval" breadcrumb={[
                {title: "项目管理"},
                {
                    title: <span style={{cursor: "pointer"}} onClick={() => {
                        navigate("/product-eval");
                    }}>试用目录</span>
                },
                {title: "试用目录明细"},
            ]}/>
            <div>
                <Space>
                    <Button
                        color="default"
                        icon={<PlusCircleOutlined style={{color: "green"}}/>}
                        onClick={(e) => {
                            productEvalEntryDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "35px 0"}}>
                <Table columns={productEvalEntryListColumns} rowKey="id"
                       dataSource={state.productEvalEntryList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productEvalEntryCount,
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
            <ProductEvalEntryDetail ref={productEvalEntryDetail} productEvalId={productEvalId} loadProductEvalEntryList={loadProductEvalEntryList}/>
        </div>
    );
};

export default ProductEvalEntry;

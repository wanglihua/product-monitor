import React, {useEffect, useRef, useState} from 'react';
import {Button, DatePicker, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined, FileExcelOutlined} from '@ant-design/icons';
import ProductServiceDetail from "./product-service-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";
import $ from "jquery";
import download from "../core/download";

const ProductService = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        nameQuery: "",
        createDateBeginQuery: "",
        createDateEndQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        productServiceList: [],
        productServiceCount: 0,
    });

    const loadProductServiceList = () => {
        post("/api/product-service/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productServiceList: res.data.productServiceList,
                    productServiceCount: res.data.productServiceCount,
                };
            });
        });
    };

    const productServiceDetail = useRef();

    const productServiceListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "项目类型",
            dataIndex: "projectTypeName",
            width: "110px",
            sorter: true,
        },
        {
            title: "学校",
            dataIndex: "collegeName",
            width: "200px",
            sorter: true,
        },
        {
            title: "产品",
            dataIndex: "productName",
            width: "250px",
            sorter: true,
        },
        {
            title: "问题类型",
            dataIndex: "problemTypeName",
            width: "110px",
            sorter: true,
        },
        {
            title: "联系老师",
            dataIndex: "contactTeacher",
            width: "120px",
            sorter: true,
        },
        {
            title: "售后次数",
            dataIndex: "serviceCount",
            width: "110px",
            sorter: true,
        },
        {
            title: "记录时间",
            dataIndex: "createTime",
            width: "150px",
            sorter: true,
        },
        {
            title: "记录人",
            dataIndex: "userName",
            width: "120px",
            sorter: true,
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const productServiceId = record.id;
                            productServiceDetail.current.show(productServiceId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-service/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductServiceList();
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
        loadProductServiceList();
    }, [JSON.stringify(listParam)]);

    const collegeNameQueryRef = useRef();
    const productNameQueryRef = useRef();
    const createDateBeginQueryRef = useRef();
    const createDateEndQueryRef = useRef();

    return (
        <div>
            <Title title="售后维护" menuKey="product-sale-service" breadcrumb={[
                {title: "产品服务"},
                {title: "售后维护"},
            ]}/>
            <div>
                <Space>
                    <Input ref={collegeNameQueryRef} addonBefore="学校名称" placeholder="学校名称查询" style={{width: 280}}/>
                    <Input ref={productNameQueryRef} addonBefore="产品名称" placeholder="产品名称查询" style={{width: 280}}/>
                    <DatePicker.RangePicker
                        placeholder={['记录时间开始', '记录时间结束']}
                        onChange={(date, dateString) => {
                            createDateBeginQueryRef.current = dateString[0];
                            createDateEndQueryRef.current = dateString[1];
                        }}
                    />
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const collegeNameQuery = collegeNameQueryRef.current.input.value;
                            const productNameQuery = productNameQueryRef.current.input.value;
                            const createDateBeginQuery = createDateBeginQueryRef.current;
                            const createDateEndQuery = createDateEndQueryRef.current;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    collegeNameQuery: collegeNameQuery,
                                    productNameQuery: productNameQuery,
                                    createDateBeginQuery: createDateBeginQuery,
                                    createDateEndQuery: createDateEndQuery,
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
                            productServiceDetail.current.show(0);
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
                                const url = `/api/product-service/export?${query}`;
                                download(url);

                                showSuccess("导出成功, 文件下载中!");
                            });
                        }}
                    >
                        导出
                    </Button>
                </Space>
            </div>
            <div style={{padding: "35px 0"}}>
                <Table columns={productServiceListColumns} rowKey="id"
                       dataSource={state.productServiceList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productServiceCount,
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
            <ProductServiceDetail ref={productServiceDetail} loadProductServiceList={loadProductServiceList}/>
        </div>
    );
};

export default ProductService;

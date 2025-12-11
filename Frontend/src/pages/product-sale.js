import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table, Upload} from 'antd';
import {FileExcelOutlined, PlusCircleOutlined, SearchOutlined, UnorderedListOutlined} from '@ant-design/icons';
import {useNavigate} from "react-router";
import ProductSaleEntryDetail from "./product-sale-entry-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";
import download from "../core/download";

const ProductSale = () => {

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
        productSaleList: [],
        productSaleCount: 0,
    });

    const loadProductSaleList = () => {
        post("/api/product-sale/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productSaleList: res.data.productSaleList,
                    productSaleCount: res.data.productSaleCount,
                };
            });
        });
    };

    const productSaleEntryDetail = useRef();

    const productSaleListColumns = [
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
            title: "销售次数 / 安装次数 / 更新次数 / 售后次数",
            dataIndex: "saleTimes",
            width: "320px",
            render: (value, record, index) => {
                return (
                    <span>
                        <span>销售&nbsp;</span>{record.saleTimes}&nbsp;/&nbsp;<span>安装&nbsp;</span>{record.installTimes}&nbsp;/&nbsp;<span>更新&nbsp;</span>{record.updateTimes}&nbsp;/&nbsp;<span>售后&nbsp;</span>{record.serviceTimes}
                    </span>
                );
            },
        },
        {
            title: "版本号",
            dataIndex: "version",
            width: "110px",
        },
        {
            title: "销售时间",
            dataIndex: "saleDate",
            width: "150px",
        },
        {
            title: "服务到期时间",
            dataIndex: "serviceExpireDate",
            width: "150px",
        },
        {
            title: "操作",
            render: (text, record, index) => {
                return (
                    <Space>
                        <Button color="primary" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            const productSaleId = record.id;
                            navigate(`/product-sale-entry/${productSaleId}`);
                        }}>
                            <UnorderedListOutlined/>销售记录
                        </Button>
                    </Space>
                );
            },
        },
    ];

    useEffect(() => {
        loadProductSaleList();
    }, [JSON.stringify(listParam)]);

    const collegeNameQueryRef = useRef();
    const productNameQueryRef = useRef();

    return (
        <div>
            <Title title="销售目录" parentMenuKeys={["project-manage"]} menuKey="product-sale" breadcrumb={[
                {title: "项目管理"},
                {title: "销售目录"},
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
                            productSaleEntryDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                    <Upload
                        showUploadList={false}
                        name="excelFile"
                        accept=".xlsx"
                        action="/api/product-sale/import"
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
                                loadProductSaleList();
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
                        const url = "/api/product-sale/export";
                        download(url);
                    }}>下载导入样板文件</a>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productSaleListColumns} rowKey="id"
                       dataSource={state.productSaleList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productSaleCount,
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
            <ProductSaleEntryDetail ref={productSaleEntryDetail} loadProductSaleList={loadProductSaleList}/>
        </div>
    );
};

export default ProductSale;

import React, {useEffect, useRef, useState} from 'react';
import {Button, Space, Table} from 'antd';
import {ReloadOutlined} from '@ant-design/icons';
import {useParams, useNavigate} from "react-router";
import ProductUrlMonitorEntryDetail from "./product-url-monitor-entry-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductUrlMonitorEntry = () => {

    const params = useParams();
    const productUrlMonitorId = params.productUrlMonitorId;

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        productUrlMonitorId: productUrlMonitorId,
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        productUrlMonitorEntryList: [],
        productUrlMonitorEntryCount: 0,
        collegeName: "",
        productName: "",
    });

    const loadProductUrlMonitorEntryList = () => {
        post("/api/product-url-monitor-entry/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productUrlMonitorEntryList: res.data.productUrlMonitorEntryList,
                    productUrlMonitorEntryCount: res.data.productUrlMonitorEntryCount,
                    collegeName: res.data.collegeName,
                    productName: res.data.productName,
                };
            });
        });
    };

    const productUrlMonitorEntryDetail = useRef();

    const productUrlMonitorEntryListColumns = [
        {
            title: "序号",
            width: "80px",
            align: "center",
            render: (text, record, index) => {
                return <span>{(listParam.currentPage - 1) * listParam.pageSize + index + 1}</span>
            },
        },
        {
            title: "使用时间",
            dataIndex: "useTime",
            width: "150px",
        },
        {
            title: "使用账号",
            dataIndex: "loginUser",
            width: "150px",
        },
        {
            title: "账号分类",
            dataIndex: "accountType",
            width: "100px",
        },
        {
            title: "使用IP",
            dataIndex: "ipAddress",
            width: "200px",
        },
        {
            title: "使用省份",
            dataIndex: "provinceType",
            width: "200px",
        },
        {
            title: ""
        },
    ];

    useEffect(() => {
        loadProductUrlMonitorEntryList();
    }, [JSON.stringify(listParam)]);

    return (
        <div>
            <Title title={`网站监测详情 学校:${state.collegeName} 产品:${state.productName}`} parentMenuKeys={["data-monitor"]} menuKey="product-url-monitor" breadcrumb={[
                {title: "数据监测"},
                {
                    title: <span style={{cursor: "pointer"}} onClick={() => {
                        navigate("/product-url-monitor");
                    }}>网站监测</span>
                },
                {title: "网站监测详情"},
            ]}/>
            <div>
                <Space>
                    <Button
                        color="default"
                        icon={<ReloadOutlined/>}
                        onClick={(e) => {
                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    current_millis: Date.now(),
                                };
                            });
                        }}
                    >
                        刷新
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productUrlMonitorEntryListColumns} rowKey="id"
                       dataSource={state.productUrlMonitorEntryList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productUrlMonitorEntryCount,
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
            <ProductUrlMonitorEntryDetail ref={productUrlMonitorEntryDetail} loadProductUrlMonitorEntryList={loadProductUrlMonitorEntryList}/>
        </div>
    );
};

export default ProductUrlMonitorEntry;

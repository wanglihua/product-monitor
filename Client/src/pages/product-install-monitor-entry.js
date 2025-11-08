import React, {useEffect, useRef, useState} from 'react';
import {Table} from 'antd';
import {useNavigate} from "react-router";
import ProductInstallMonitorEntryDetail from "./product-install-monitor-entry-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductInstallMonitorEntry = () => {

    const {post, showError} = useSiteContext();

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
        productInstallMonitorEntryList: [],
        productInstallMonitorEntryCount: 0,
    });

    const loadProductInstallMonitorEntryList = () => {
        post("/api/product-install-monitor-entry/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productInstallMonitorEntryList: res.data.productInstallMonitorEntryList,
                    productInstallMonitorEntryCount: res.data.productInstallMonitorEntryCount,
                };
            });
        });
    };

    const productInstallMonitorEntryDetail = useRef();

    const productInstallMonitorEntryListColumns = [
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
            dataIndex: "createTime",
            width: "150px",
        },
        {
            title: "使用账号",
            dataIndex: "loginUser",
            width: "200px",
        },
        {
            title: "账号分类",
            dataIndex: "accountType",
            width: "100px",
        },
        {
            title: "使用IP",
            dataIndex: "ipAddress",
            width: "150px",
        },
        {
            title: "使用省份",
            dataIndex: "provinceType",
            width: "150px",
        },
        {
            title: "",
            dataIndex: "",
        },
    ];

    useEffect(() => {
        loadProductInstallMonitorEntryList();
    }, [JSON.stringify(listParam)]);

    return (
        <div>
            <Title title="院校使用详情" parentMenuKeys={["data-monitor"]} menuKey="product-install-monitor" breadcrumb={[
                {title: "数据监测"},
                {
                    title: <span style={{cursor: "pointer"}} onClick={() => {
                        navigate("/product-install-monitor");
                    }}>安装监测</span>
                },
                {title: "院校使用详情"},
            ]}/>
            <div style={{padding: "10px 0"}}>
                <Table columns={productInstallMonitorEntryListColumns} rowKey="id"
                       dataSource={state.productInstallMonitorEntryList}
                       pagination={{
                           current: listParam.currentPage,
                           pageSize: listParam.pageSize,
                           total: state.productInstallMonitorEntryCount,
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
            <ProductInstallMonitorEntryDetail ref={productInstallMonitorEntryDetail} loadProductInstallMonitorEntryList={loadProductInstallMonitorEntryList}/>
        </div>
    );
};

export default ProductInstallMonitorEntry;

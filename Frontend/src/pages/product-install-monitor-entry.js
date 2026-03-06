import React, {useEffect, useRef, useState} from 'react';
import {Button, Space, Table} from 'antd';
import {ReloadOutlined} from "@ant-design/icons";
import {useParams, useNavigate} from "react-router";
import useSiteContext from "../site/site-context";
import Title from "../site/title";
import ProductInstallMonitorEntryDetail from "./product-install-monitor-entry-detail";

const ProductInstallMonitorEntry = () => {

    const params = useParams();
    const productInstallMonitorId = params.productInstallMonitorId;

    const {post, showError} = useSiteContext();

    const navigate = useNavigate();

    const [listParam, setListParam] = useState({
        productInstallMonitorId: productInstallMonitorId,
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
        current_millis: Date.now(),
    });

    const [state, setState] = useState({
        productInstallMonitorEntryList: [],
        productInstallMonitorEntryCount: 0,
        collegeName: "",
        productName: "",
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
                    collegeName: res.data.collegeName,
                    productName: res.data.productName,
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
        },
    ];

    useEffect(() => {
        loadProductInstallMonitorEntryList();
    }, [JSON.stringify(listParam)]);

    return (
        <div>
            <Title title={`院校使用详情 学校:${state.collegeName} 产品:${state.productName}`} parentMenuKeys={["data-monitor"]} menuKey="product-install-monitor" breadcrumb={[
                {title: "数据监测"},
                {
                    title: <span style={{cursor: "pointer"}} onClick={() => {
                        navigate("/product-install-monitor");
                    }}>安装监测</span>
                },
                {title: "院校使用详情"},
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
            <div style={{padding: "35px 0"}}>
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

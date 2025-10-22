import React, {useEffect, useRef, useState} from 'react';
import {Button, Input, Space, Table} from 'antd';
import {EditOutlined, DeleteOutlined, PlusCircleOutlined, SearchOutlined} from '@ant-design/icons';
import ProductInstallDetail from "./product-install-detail";

import useSiteContext from "../site/site-context";
import Title from "../site/title";

const ProductInstall = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    const [listParam, setListParam] = useState({
        nameQuery: "",
        currentPage: 1,
        pageSize: 15,
        sortField: "id",
        sortOrder: "descend",
    });

    const [state, setState] = useState({
        productInstallList: [],
        productInstallCount: 0,
    });

    const loadProductInstallList = () => {
        post("/api/product-install/list", listParam, function (response) {
            const res = response.data;
            if (res.success !== true) {
                showError(res.message);
                return;
            }

            setState((pre) => {
                return {
                    ...pre,
                    productInstallList: res.data.productInstallList,
                    productInstallCount: res.data.productInstallCount,
                };
            });
        });
    };

    const productInstallDetail = useRef();

    const productInstallListColumns = [
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
            dataIndex: "collegeId",
            width: "200px",
        },
        {
            title: "产品",
            dataIndex: "productId",
            width: "200px",
        },
        {
            title: "销售版本",
            dataIndex: "versionSale",
            width: "200px",
        },
        {
            title: "安装版本",
            dataIndex: "versionInstall",
            width: "200px",
        },
        {
            title: "安装时间",
            dataIndex: "installDate",
            width: "200px",
        },
        {
            title: "更新网址",
            dataIndex: "updateUrl",
            width: "200px",
        },
        {
            title: "系统账号",
            dataIndex: "loginUser",
            width: "200px",
        },
        {
            title: "系统密码",
            dataIndex: "loginPassword",
            width: "200px",
        },
        {
            title: "联系教师",
            dataIndex: "contactTeacher",
            width: "200px",
        },
        {
            title: "联系电话",
            dataIndex: "contactMobile",
            width: "200px",
        },
        {
            title: "其他备注",
            dataIndex: "description",
            width: "200px",
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
                            const productInstallId = record.id;
                            productInstallDetail.current.show(productInstallId);
                        }}>
                            <EditOutlined/>编辑
                        </Button>
                        <Button color="danger" variant="text" size="small" style={{fontSize: "12px"}} onClick={(e) => {
                            showConfirm("确认要删除吗！", "确认", () => {
                                post("/api/product-install/delete", {
                                    id: record.id,
                                }, function (response) {
                                    const res = response.data;
                                    if (res.success !== true) {
                                        showError(res.message);
                                        return;
                                    }

                                    showSuccess(res.message);
                                    loadProductInstallList();
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
        loadProductInstallList();
    }, [JSON.stringify(listParam)]);

    const nameQueryRef = useRef();

    return (
        <div>
            <Title title="产品安装" menuKey="product-install" breadcrumb={[
                {title: "产品安装"},
            ]}/>
            <div>
                <Space>
                    <Input ref={nameQueryRef} addonBefore="名称" placeholder="名称查询" style={{width: 320}}/>
                    <Button
                        color="default"
                        icon={<SearchOutlined/>}
                        onClick={(e) => {
                            const nameQuery = nameQueryRef.current.input.value;

                            setListParam((pre) => {
                                return {
                                    ...pre,
                                    nameQuery: nameQuery,
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
                            productInstallDetail.current.show(0);
                        }}
                    >
                        添加
                    </Button>
                </Space>
            </div>
            <div style={{padding: "10px 0"}}>
                <Table columns={productInstallListColumns} rowKey="id"
                       dataSource={state.productInstallList}
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
            <ProductInstallDetail ref={productInstallDetail} loadProductInstallList={loadProductInstallList}/>
        </div>
    );
};

export default ProductInstall;

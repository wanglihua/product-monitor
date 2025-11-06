import React from "react";
import {Route, Routes} from "react-router";
import Login from "./site/login";

import LayoutSite from "./site/layout-site";
import LayoutAdmin from "./site/layout-admin";
import PageNotFound from "./site/page-not-found";
import Test from "./pages/test";
import User from "./pages/user";
import Home from "./pages/home";
import AdminUser from "./pages/admin-user";
import ActionLog from "./pages/action-log";
import Product from "./pages/product";
import ProductVersion from "./pages/product-version";
import College from "./pages/college";
import ProductUrl from "./pages/product-url";
import ProductSale from "./pages/product-sale";
import ProductSaleEntry from "./pages/product-sale-entry";
import ProductRent from "./pages/product-rent";
import ProductRentEntry from "./pages/product-rent-entry";
import ProductEval from "./pages/product-eval";
import ProductEvalEntry from "./pages/product-eval-entry";
import ProductInstall from "./pages/product-install";
import ProductUpdate from "./pages/product-update";
import ProductService from "./pages/product-service";
import ProductSpecialService from "./pages/product-special-service";
import ProductInstallMonitor from "./pages/product-install-monitor";
import ProductInstallMonitorEntry from "./pages/product-install-monitor-entry";
import ProductUrlMonitor from "./pages/product-url-monitor";
import ExceptionMonitor from "./pages/exception-monitor";

const Router = () => {
    return (
        <Routes>
            <Route element={<LayoutSite/>}>
                <Route path="/login" element={<Login/>}/>
                <Route element={<LayoutAdmin/>}>
                    <Route path="/" element={<Home/>}/>
                    <Route path="/test" element={<Test/>}/>
                    <Route path="/product" element={<Product/>}/>
                    <Route path="/product-version/:productId" element={<ProductVersion/>}/>
                    <Route path="/college" element={<College/>}/>
                    <Route path="/product-url" element={<ProductUrl/>}/>
                    <Route path="/product-sale" element={<ProductSale/>}/>
                    <Route path="/product-sale-entry/:productSaleId" element={<ProductSaleEntry/>}/>
                    <Route path="/product-rent" element={<ProductRent/>}/>
                    <Route path="/product-rent-entry/:productRentId" element={<ProductRentEntry/>}/>
                    <Route path="/product-eval" element={<ProductEval/>}/>
                    <Route path="/product-eval-entry/:productEvalId" element={<ProductEvalEntry/>}/>
                    <Route path="/product-install" element={<ProductInstall/>}/>
                    <Route path="/product-update" element={<ProductUpdate/>}/>
                    <Route path="/product-service" element={<ProductService/>}/>
                    <Route path="/product-special-service" element={<ProductSpecialService/>}/>
                    <Route path="/product-install-monitor" element={<ProductInstallMonitor/>}/>
                    <Route path="/product-install-monitor-entry/:productInstallMonitorId" element={<ProductInstallMonitorEntry/>}/>
                    <Route path="/product-url-monitor" element={<ProductUrlMonitor/>}/>
                    <Route path="/exception-monitor" element={<ExceptionMonitor/>}/>
                    <Route path="/user" element={<User/>}/>
                    <Route path="/admin/user" element={<AdminUser/>}/>
                    <Route path="/action/log" element={<ActionLog/>}/>
                </Route>

                <Route path="*" element={<PageNotFound/>}/>
            </Route>
        </Routes>
    );
}

export default Router;

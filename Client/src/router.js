import React from "react";
import {Route, Routes} from "react-router";
import Login from "./site/login";

import LayoutSite from "./site/layout-site";
import LayoutAdmin from "./site/layout-admin";
import PageNotFound from "./site/page-not-found";
import User from "./pages/user";
import Home from "./pages/home";
import AdminUser from "./pages/admin-user";
import ActionLog from "./pages/action-log";
import Product from "./pages/product";
import College from "./pages/college";
import ProductUrl from "./pages/product-url";
import ProductSale from "./pages/product-sale";
import ProductRent from "./pages/product-rent";
import ProductEval from "./pages/product-eval";

const Router = () => {
    return (
        <Routes>
            <Route element={<LayoutSite/>}>
                <Route path="/login" element={<Login/>}/>
                <Route element={<LayoutAdmin/>}>
                    <Route path="/" element={<Home/>}/>
                    <Route path="/product" element={<Product/>}/>
                    <Route path="/college" element={<College/>}/>
                    <Route path="/product-url" element={<ProductUrl/>}/>
                    <Route path="/product-sale" element={<ProductSale/>}/>
                    <Route path="/product-rent" element={<ProductRent/>}/>
                    <Route path="/product-eval" element={<ProductEval/>}/>
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

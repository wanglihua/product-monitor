import React from "react";
import { Route, Routes } from "react-router";
import Login from "./site/login";

import LayoutSite from "./site/layout-site";
import LayoutAdmin from "./site/layout-admin";
import LayoutUser from "./site/layout-user";
import PageNotFound from "./site/page-not-found";
import AdminChat from "./admin/chat";
import AdminChatMessage from "./admin/chat-message";
import PresetQuestion from "./admin/preset-question";
import User from "./admin/user";
import AdminHome from "./admin/admin-home";
import AdminUser from "./admin/admin-user";
import UserLoginLog from "./admin/user-login-log";

import Home from "./user/user-home";
import ChatIndex from "./user/chat-index";
import ChatMessage from "./user/chat-message";
import ToolIndex from "./user/tool-index";

const Router = () => {
    return (
        <Routes>
            <Route element={<LayoutSite />}>
                <Route path="/login" element={<Login />} />

                <Route element={<LayoutUser />}>
                    <Route index exact element={<Home />} />
                    <Route path="/chat" element={<ChatIndex />} />
                    <Route path="/chat/message/:chatId" element={<ChatMessage />} />
                    <Route path="/tool" element={<ToolIndex />} />
                </Route>

                <Route element={<LayoutAdmin />}>
                    <Route path="/admin" element={<AdminHome />} />
                    <Route path="/admin/chat" element={<AdminChat />} />
                    <Route path="/admin/chat/message/:chatId" element={<AdminChatMessage />} />
                    <Route path="/admin/preset/question" element={<PresetQuestion />} />
                    <Route path="/admin/user" element={<User />} />
                    <Route path="/admin/admin-user" element={<AdminUser />} />
                    <Route path="/admin/user/login/log" element={<UserLoginLog />} />
                </Route>

                <Route path="*" element={<PageNotFound />} />
            </Route>
        </Routes>
    );
}

export default Router;

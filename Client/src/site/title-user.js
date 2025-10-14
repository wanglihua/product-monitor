import React, { useEffect } from 'react';

import { getTitle } from "../core/config";
import useSiteContext from "./site-context";
import { isNotEmptyArray } from "../core/empty";

const TitleUser = (props) => {
    const title = props.title;
    const breadcrumb = props.breadcrumb;
    const menuKey = props.menuKey ? props.menuKey : "";

    const { setBreadcrumb, setMenuActiveKey } = useSiteContext();

    useEffect(() => {
        document.title = getTitle(title);

        if (isNotEmptyArray(breadcrumb)) {
            setBreadcrumb(breadcrumb);
        }
        else {
            setBreadcrumb([]);
        }

    }, [title]);

    useEffect(() => {
        setMenuActiveKey(menuKey);
    }, [menuKey]);

    // 暂时不显示
    return (
        <></>
    );
};

export default TitleUser;
import React, { useEffect } from 'react';

import { getTitle } from "../core/config";
import useSiteContext from "./site-context";
import { isEmptyStr, isNotEmptyArray } from "../core/empty";

const Title = (props) => {
    const title = props.title;
    const breadcrumb = props.breadcrumb;
    const menuKey = props.menuKey ? props.menuKey : "";
    const displayStyle = isEmptyStr(title) ? "none" : "block";

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

    return (
        <h1 className="page-title" style={{ display: displayStyle }}>
            {title}
        </h1>
    );
};

export default Title;
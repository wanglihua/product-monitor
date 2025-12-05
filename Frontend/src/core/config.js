import { isEmptyStr } from "./empty";

const titleSuffix = "管理平台";

const getTitle = (title) => { 
    if(isEmptyStr(title)) {
        return titleSuffix;
    } else {
        return `${title} - ${titleSuffix}`;
    }
};

export { getTitle };
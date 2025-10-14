import { isEmptyStr } from "./empty";

const titleSuffix = "AI智能助手";

const getTitle = (title) => { 
    if(isEmptyStr(title)) {
        return titleSuffix;
    } else {
        return `${title} - ${titleSuffix}`;
    }
};

export { getTitle };
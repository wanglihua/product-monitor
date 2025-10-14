const isEmptyNum = (num) => {
    if (num === undefined) {
        return true;
    }

    if (num === null) {
        return true;
    }

    return num === 0;
};

const isNotEmptyNum = (num) => {
    return isEmptyNum(num) === false;
};

const isEmptyStr = (str) => {
    if (str === undefined) {
        return true;
    }

    if (str === null) {
        return true;
    }

    return str === "";
};

const isNotEmptyStr = (str) => {
    return isEmptyStr(str) === false;
};

const isEmptyArray = (arr) => {
    if (arr === undefined) {
        return true;
    }

    if (arr === null) {
        return true;
    }

    return arr.length === 0;
};

const isNotEmptyArray = (arr) => {
    return isEmptyArray(arr) === false;
};

const isEmptyObj = (obj) => {
    if (obj === undefined) {
        return true;
    }

    if (obj === null) {
        return true;
    }

    return false;
};

const isNotEmptyObj = (obj) => {
    return isEmptyObj(obj) === false;
};

export { isEmptyNum, isNotEmptyNum, isEmptyStr, isNotEmptyStr, isEmptyArray, isNotEmptyArray, isEmptyObj, isNotEmptyObj };

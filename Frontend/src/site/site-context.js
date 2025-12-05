import { createContext, useContext } from "react";

const SiteContext = createContext({
    // 这两个可以不需要，后续有新的状态值，也不用在这里添加类似成员
    // router.js 里面 <SiteContext.Provider value={siteContextValue} /> 中的siteContextValue中提供相应成员就可以
    // mask: false,
    // setMask: () => { },
});

const useSiteContext = () => {
    return useContext(SiteContext);
};

export default useSiteContext;
export { SiteContext };


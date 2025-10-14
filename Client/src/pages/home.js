import Title from "../site/title";

const Home = () => {

    /*
    
        // 后面不要忘了，加上这个判断，然后 useEffect() 依赖于这个变量
        if (user.userId !== 0) {
            post("/api/admin/home/page/data", {}, function (response) {
                const res = response.data;
                if (res.success !== true) {
                    showError(res.message);
                    return;
                }

                setState((pre) => {
                    return {
                        ...pre,
                        greeting: res.data.greeting,
                    };
                });
            });
        }

    */

    return (
        <div>
            <Title title="首页" />
        </div>
    );
};

export default Home;
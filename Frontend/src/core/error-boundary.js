import React from "react";

class ErrorBoundary extends React.Component {
    constructor(props) {
        super(props);
        this.state = {
            hasError: false,
            error: "",
            info: "",
        };
    }

    componentDidCatch(error, info) {
        // 显示错误UI
        this.setState({
            hasError: true,
            error: error,
            info: info,
        });

        // 同时输出错误日志
        console.log(error);
        console.log(info);
    }

    render() {
        const hasError = this.state.hasError;
        const error = this.state.error;
        const info = this.state.info;

        if (hasError) {
            return <div style={{ margin: "20px" }}>
                <h1>当前页面发生严重错误，请在首页联系客服！</h1>
                <div style={{ padding: "10px 0" }}>
                    <a href="/">返回首页</a>
                </div>
                <div>
                    {error.message}
                </div>
                <div>
                    <pre>
                        {info.componentStack}
                    </pre>
                </div>
            </div>;
        }
        return this.props.children;
    }
}

export default ErrorBoundary;

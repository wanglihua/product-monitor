import {Image} from "antd";
import React from "react";

class Captcha extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            random: (new Date()).getTime(),
        };
    }

    // 供外部调用
    refresh() {
        this.setState({
            random: (new Date()).getTime(),
        });
    }

    render() {
        const random = this.state.random;

        return (
            <Image alt="验证码"
                   src={`/api/captcha?${random}`}
                   onClick={() => {
                       this.setState({
                           random: (new Date()).getTime(),
                       });
                   }}
                   preview={false}
                   width={72}
                   height={32}
                   style={{cursor: "pointer"}}
            />
        );
    }

    componentDidMount() {
        // this.setState({
        //     random: (new Date()).getTime(),
        // });

        let self = this;
        window.setTimeout(() => {
            self.setState({
                random: (new Date()).getTime(),
            });
        }, 1000);
    }
}

export default Captcha;

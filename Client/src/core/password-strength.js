import React from "react";

class PasswordStrength extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            password: "",
        };
    }

    setPassword(password) {
        this.setState({
            password: password,
        });
    }

    securityLevel(password) {
        if (password.length < 6) {
            return 0;
        }

        let level = 0;
        if (/\d/.test(password)) {
            level++;
        }

        if (/[a-z]/.test(password)) {
            level++;
        }

        if (/[A-Z]/.test(password)) {
            level++;
        }

        if (/\W/.test(password)) {
            level++;
        }

        return level;
    }

    render() {
        const password = this.state.password;
        let level = this.securityLevel(password);

        let bgWith = ((level + 1) * 25) + "%";

        let bgColor = "red";
        if (level === 1) {
            bgColor = "#FF9900";
        } else if (level === 2) {
            bgColor = "#FFCC00";
        } else if (level === 3) {
            bgColor = "#339933";
        } else if (level === 4) {
            bgColor = "green";
        }

        return (<div style={{ marginTop: "5px", fontSize: "1px", height: "8px" }}>
            <div style={{ display: "inline-block", width: "100%", border: "1px solid lightgray", textAlign: "center" }}>
                <div style={{ width: bgWith, backgroundColor: bgColor, height: "5px" }}>&nbsp;</div>
            </div>
        </div>);
    }
};

export default PasswordStrength;
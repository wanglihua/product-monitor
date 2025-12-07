import { theme } from "antd";
import {
    CopyOutlined,
} from '@ant-design/icons';
import copyText from "../core/clipboard";
import useSiteContext from "./site-context";

const Copy = ({ text }) => {
    const { token } = theme.useToken();
    const { showSuccess } = useSiteContext();

    const notEmptyText = text && text !== "-";
    const isEmptyText = !(notEmptyText);

    return (
        <>
            {notEmptyText && (
                <span style={{ color: "rgb(22, 119, 255)", cursor: "pointer" }} onClick={(e) => {
                    copyText(text);
                    showSuccess(`复制 ${text} 成功!`, "成功", 1);
                }}>
                    {text}&nbsp;<CopyOutlined />
                </span>
            )}
            {isEmptyText && (
                <span>{text}</span>
            )}

        </>
    );
}

export default Copy;

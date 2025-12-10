import $ from "jquery";

const download = (url) => {
    $(`<form method="post" action="${url}"></form>`).appendTo('body').submit().remove();
};
export default download;
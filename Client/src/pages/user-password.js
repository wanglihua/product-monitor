import useSiteContext from "../site/site-context";
import Title from "../site/title";

const UserPassword = () => {

    const {post, showConfirm, showError, showSuccess} = useSiteContext();

    return (
        <div>
            <Title title="修改密码" breadcrumb={[
                {title: "修改密码"},
            ]}/>
        </div>
    );
}

export default UserPassword;
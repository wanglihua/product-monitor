CREATE TABLE tbl_product_update
(
    id              BIGINT        NOT NULL,

    college_id      BIGINT        NOT NULL,
    product_id      BIGINT        NOT NULL,

    version_sale    NVARCHAR(32)  NOT NULL,
    version_update  NVARCHAR(32)  NOT NULL,
    update_count    INT           NOT NULL,
    update_url      NVARCHAR(256) NOT NULL,
    login_user      NVARCHAR(128) NOT NULL,
    login_password  NVARCHAR(128) NOT NULL,
    update_date     DATETIME      NOT NULL,
    sales_manager   NVARCHAR(32)  NOT NULL,
    contact_teacher NVARCHAR(32)  NOT NULL,
    contact_mobile  NVARCHAR(32)  NOT NULL,

    description     NVARCHAR(512) NOT NULL,

    user_id         BIGINT        NOT NULL,

    create_time     DATETIME      NOT NULL,
    update_time     DATETIME      NOT NULL,

    CONSTRAINT pk_product_update PRIMARY KEY (id),
    CONSTRAINT fk_product_update_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_product_update_college_id (college_id),
    CONSTRAINT fk_product_update_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_update_product_id (product_id),
    CONSTRAINT fk_product_update_user_id FOREIGN KEY (user_id) REFERENCES tbl_user (id),
);

-- DROP TABLE tbl_product_update;

EXEC sp_AddExtendedProperty 'MS_Description', N'产品更新', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'学校', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'college_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'销售版本', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'version_sale';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新版本', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'version_update';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新次数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'update_count';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新网址', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'update_url';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新账号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'login_user';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新密码', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'login_password';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'update_date';
EXEC sp_AddExtendedProperty 'MS_Description', N'业务经理', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'sales_manager';
EXEC sp_AddExtendedProperty 'MS_Description', N'联系老师', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'contact_teacher';
EXEC sp_AddExtendedProperty 'MS_Description', N'联系电话', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'contact_mobile';
EXEC sp_AddExtendedProperty 'MS_Description', N'其他备注', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'description';
EXEC sp_AddExtendedProperty 'MS_Description', N'记录人', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'user_id';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_update', 'COLUMN', 'update_time';
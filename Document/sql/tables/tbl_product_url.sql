CREATE TABLE tbl_product_url
(
    id             BIGINT        NOT NULL,

    product_id     BIGINT        NOT NULL,
    url            NVARCHAR(256) NOT NULL,
    type_code      INT           NOT NULL,

    login_user     NVARCHAR(128) NOT NULL,
    login_password NVARCHAR(128) NOT NULL,

    description    NVARCHAR(512) NOT NULL,

    create_time    DATETIME      NOT NULL,
    update_time    DATETIME      NOT NULL,

    CONSTRAINT pk_product_url PRIMARY KEY (id),

    CONSTRAINT fk_product_url_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_url_product_id (product_id),
);

-- DROP TABLE tbl_product_url;

EXEC sp_AddExtendedProperty 'MS_Description', N'网址管理', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'网址', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url', 'COLUMN', 'url';
EXEC sp_AddExtendedProperty 'MS_Description', N'类型', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url', 'COLUMN', 'type_code';
EXEC sp_AddExtendedProperty 'MS_Description', N'账号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url', 'COLUMN', 'login_user';
EXEC sp_AddExtendedProperty 'MS_Description', N'密码', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url', 'COLUMN', 'login_password';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url', 'COLUMN', 'update_time';


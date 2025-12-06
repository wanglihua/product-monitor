CREATE TABLE tbl_client_key
(
    id          BIGINT        NOT NULL,

    college_id  BIGINT        NOT NULL,
    product_id  BIGINT        NOT NULL,

    key_type    INT           NOT NULL CONSTRAINT df_client_key_key_type DEFAULT 0,

    client_key  NVARCHAR(128) NOT NULL,

    user_id     BIGINT        NOT NULL,

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_client_key PRIMARY KEY (id),
    CONSTRAINT fk_client_key_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_client_key_college_id (college_id),
    CONSTRAINT fk_client_key_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_client_key_product_id (product_id),
    CONSTRAINT fk_client_key_user_id FOREIGN KEY (user_id) REFERENCES tbl_user (id),
    INDEX ix_client_key_key (client_key),
);

EXEC sp_AddExtendedProperty 'MS_Description', N'客户端密钥', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'学校', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'college_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'类型', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'key_type';
EXEC sp_AddExtendedProperty 'MS_Description', N'密钥', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'client_key';
EXEC sp_AddExtendedProperty 'MS_Description', N'操作人', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'user_id';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'update_time';
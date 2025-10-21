CREATE TABLE tbl_product_version
(
    id          BIGINT        NOT NULL,
    product_id  BIGINT        NOT NULL,

    major       INT           NOT NULL,
    minor       INT           NOT NULL,
    revision    INT           NOT NULL,

    description NVARCHAR(256) NOT NULL,

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_product_version PRIMARY KEY (id),
    CONSTRAINT fk_product_version_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_version_product_id (product_id)
);

EXEC sp_AddExtendedProperty 'MS_Description', N'产品版本', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'产品目录', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'主版本号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'major';
EXEC sp_AddExtendedProperty 'MS_Description', N'次版本号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'minor';
EXEC sp_AddExtendedProperty 'MS_Description', N'修订号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'revision';
EXEC sp_AddExtendedProperty 'MS_Description', N'描述', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'description';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'update_time';
CREATE TABLE tbl_product_sale_entry
(
    id                  BIGINT        NOT NULL,
    product_sale_id     BIGINT        NOT NULL,

    version             NVARCHAR(32)  NOT NULL,

    sale_date           DATETIME      NOT NULL,
    service_expire_date DATETIME      NOT NULL,

    description         NVARCHAR(512) NOT NULL,

    create_time         DATETIME      NOT NULL,
    update_time         DATETIME      NOT NULL,

    CONSTRAINT pk_product_sale_entry PRIMARY KEY (id),

    CONSTRAINT fk_product_sale_entry_product_sale_id FOREIGN KEY (product_sale_id) REFERENCES tbl_product_sale (id),
    INDEX ix_product_sale_entry_product_sale_id (product_sale_id),
);

-- DROP TABLE tbl_product_sale_entry;

EXEC sp_AddExtendedProperty 'MS_Description', N'销售目录明细', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'销售目录 ', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'product_sale_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'版本号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'version';
EXEC sp_AddExtendedProperty 'MS_Description', N'销售时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'sale_date';
EXEC sp_AddExtendedProperty 'MS_Description', N'服务到期时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'service_expire_date';
EXEC sp_AddExtendedProperty 'MS_Description', N'备注', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'description';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'update_time';
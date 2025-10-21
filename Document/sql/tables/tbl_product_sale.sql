CREATE TABLE tbl_product_sale
(
    id                  BIGINT        NOT NULL,
    college_id          BIGINT        NOT NULL,
    product_id          BIGINT        NOT NULL,

    sale_times          INT           NOT NULL,
    install_times       INT           NOT NULL,
    update_times        INT           NOT NULL,
    service_times       INT           NOT NULL,

    version             NVARCHAR(32)  NOT NULL,

    sale_date           DATETIME      NOT NULL,
    service_expire_date DATETIME      NOT NULL,

    description         NVARCHAR(512) NOT NULL,

    create_time         DATETIME      NOT NULL,
    update_time         DATETIME      NOT NULL,

    CONSTRAINT pk_product_sale PRIMARY KEY (id),
    CONSTRAINT fk_product_sale_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_sale_product_id (product_id),
    CONSTRAINT fk_product_sale_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_college_sale_college_id (college_id),
);

-- DROP TABLE tbl_product_sale;

EXEC sp_AddExtendedProperty 'MS_Description', N'销售目录', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'学校', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'college_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'销售次数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'sale_times';
EXEC sp_AddExtendedProperty 'MS_Description', N'安装次数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'install_times';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新次数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'update_times';
EXEC sp_AddExtendedProperty 'MS_Description', N'售后次数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'service_times';
EXEC sp_AddExtendedProperty 'MS_Description', N'版本号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'version';
EXEC sp_AddExtendedProperty 'MS_Description', N'销售时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'sale_date';
EXEC sp_AddExtendedProperty 'MS_Description', N'服务到期时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'service_expire_date';
EXEC sp_AddExtendedProperty 'MS_Description', N'备注', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'description';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale', 'COLUMN', 'update_time';
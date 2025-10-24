CREATE TABLE tbl_product_rent
(
    id            BIGINT        NOT NULL,
    college_id    BIGINT        NOT NULL,
    product_id    BIGINT        NOT NULL,

    rent_times    INT           NOT NULL,
    service_times INT           NOT NULL,
    total_days    INT           NOT NULL,
    remain_days   INT           NOT NULL,

    begin_date    DATETIME      NOT NULL,
    end_date      DATETIME      NOT NULL,

    description   NVARCHAR(512) NOT NULL,

    create_time   DATETIME      NOT NULL,
    update_time   DATETIME      NOT NULL,

    CONSTRAINT pk_product_rent PRIMARY KEY (id),
    CONSTRAINT fk_product_rent_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_rent_product_id (product_id),
    CONSTRAINT fk_product_rent_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_college_rent_college_id (college_id),
);

-- DROP TABLE tbl_product_rent;

EXEC sp_AddExtendedProperty 'MS_Description', N'租赁目录', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'学校', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'college_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'租赁次数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'rent_times';
EXEC sp_AddExtendedProperty 'MS_Description', N'售后次数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'service_times';
EXEC sp_AddExtendedProperty 'MS_Description', N'累计天数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'total_days';
EXEC sp_AddExtendedProperty 'MS_Description', N'剩余天数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'remain_days';
EXEC sp_AddExtendedProperty 'MS_Description', N'开始时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'begin_date';
EXEC sp_AddExtendedProperty 'MS_Description', N'结束时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'end_date';
EXEC sp_AddExtendedProperty 'MS_Description', N'备注', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'description';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_rent', 'COLUMN', 'update_time';
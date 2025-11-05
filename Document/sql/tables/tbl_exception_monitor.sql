CREATE TABLE tbl_exception_monitor
(
    id            BIGINT       NOT NULL,

    product_id    BIGINT       NOT NULL,

    ip_address    NVARCHAR(32) NOT NULL,
    province_type INT          NOT NULL DEFAULT 0,
    port          INT          NOT NULL,

    create_time   DATETIME     NOT NULL,
    update_time   DATETIME     NOT NULL,

    CONSTRAINT pk_exception_monitor PRIMARY KEY (id)
);

EXEC sp_AddExtendedProperty 'MS_Description', N'异常检测', 'SCHEMA', 'dbo', 'TABLE', 'tbl_exception_monitor';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_exception_monitor', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_exception_monitor', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'使用IP', 'SCHEMA', 'dbo', 'TABLE', 'tbl_exception_monitor', 'COLUMN', 'ip_address';
EXEC sp_AddExtendedProperty 'MS_Description', N'使用省份', 'SCHEMA', 'dbo', 'TABLE', 'tbl_exception_monitor', 'COLUMN', 'province_type';
EXEC sp_AddExtendedProperty 'MS_Description', N'使用端口', 'SCHEMA', 'dbo', 'TABLE', 'tbl_exception_monitor', 'COLUMN', 'port';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_exception_monitor', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_exception_monitor', 'COLUMN', 'update_time';
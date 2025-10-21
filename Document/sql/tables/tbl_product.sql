CREATE TABLE tbl_product
(
    id                  BIGINT        NOT NULL,

    name                NVARCHAR(256) NOT NULL,
    version             NVARCHAR(32)  NOT NULL,
    version_update_base NVARCHAR(32)  NOT NULL,
    version_update_desc NVARCHAR(512) NOT NULL,
    version_update_time DATETIME      NOT NULL,

    create_time         DATETIME      NOT NULL,
    update_time         DATETIME      NOT NULL,

    CONSTRAINT pk_product PRIMARY KEY (id)
);

EXEC sp_AddExtendedProperty 'MS_Description', N'产品目录', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'产品名称', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product', 'COLUMN', 'name';
EXEC sp_AddExtendedProperty 'MS_Description', N'版本号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product', 'COLUMN', 'version';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新基础', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product', 'COLUMN', 'version_update_base';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新内容', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product', 'COLUMN', 'version_update_desc';
EXEC sp_AddExtendedProperty 'MS_Description', N'版本更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product', 'COLUMN', 'version_update_time';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product', 'COLUMN', 'update_time';
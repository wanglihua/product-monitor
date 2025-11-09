CREATE TABLE tbl_test
(
    id          BIGINT        NOT NULL,
    name        NVARCHAR(128) NOT NULL,

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_test PRIMARY KEY (id)
);

EXEC sp_AddExtendedProperty 'MS_Description', N'测试表', 'SCHEMA', 'dbo', 'TABLE', 'tbl_test';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_test', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'名称', 'SCHEMA', 'dbo', 'TABLE', 'tbl_test', 'COLUMN', 'name';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_test', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_test', 'COLUMN', 'update_time';
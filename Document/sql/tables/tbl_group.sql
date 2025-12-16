CREATE TABLE tbl_group
(
    id          BIGINT        NOT NULL,

    name        NVARCHAR(128) NOT NULL,

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_group PRIMARY KEY (id)
);

CREATE UNIQUE INDEX ix_group_name ON tbl_group (name);

EXEC sp_AddExtendedProperty 'MS_Description', N'群组', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'群组名称', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group', 'COLUMN', 'name';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group', 'COLUMN', 'update_time';
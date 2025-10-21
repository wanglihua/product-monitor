CREATE TABLE tbl_college
(
    id          BIGINT        NOT NULL,

    code        NVARCHAR(64)  NOT NULL,
    name        NVARCHAR(128) NOT NULL,
    province    NVARCHAR(32)  NOT NULL,
    level       NVARCHAR(32)  NOT NULL, -- 暂时使用字符串

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_college PRIMARY KEY (id),
    INDEX ix_college_code (code)
);

EXEC sp_AddExtendedProperty 'MS_Description', N'院校目录', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'院校标识码', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'code';
EXEC sp_AddExtendedProperty 'MS_Description', N'院校名称', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'name';
EXEC sp_AddExtendedProperty 'MS_Description', N'身份', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'province';
EXEC sp_AddExtendedProperty 'MS_Description', N'办学层次', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'level';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'update_time';
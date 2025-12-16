CREATE TABLE tbl_college
(
    id          BIGINT        NOT NULL,

    code        NVARCHAR(64)  NOT NULL,
    name        NVARCHAR(128) NOT NULL,
    province    INT           NOT NULL CONSTRAINT df_college_province DEFAULT 0,
    level       INT           NOT NULL CONSTRAINT df_college_level DEFAULT 0,
    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_college PRIMARY KEY (id),
    INDEX ix_college_code (code),
);

-- CREATE UNIQUE INDEX ix_college_code ON tbl_college (code);
CREATE UNIQUE INDEX ix_college_name ON tbl_college (name);

EXEC sp_AddExtendedProperty 'MS_Description', N'院校目录', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'院校标识码', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'code';
EXEC sp_AddExtendedProperty 'MS_Description', N'院校名称', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'name';
EXEC sp_AddExtendedProperty 'MS_Description', N'省份', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'province';
EXEC sp_AddExtendedProperty 'MS_Description', N'办学层次', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'level';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college', 'COLUMN', 'update_time';


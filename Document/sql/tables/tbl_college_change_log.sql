CREATE TABLE tbl_college_change_log
(
    id          BIGINT        NOT NULL,

    college_id  BIGINT        NOT NULL,

    change_desc NVARCHAR(512) NOT NULL CONSTRAINT df_college_change_log_change_desc DEFAULT '',

    create_time DATETIME      NOT NULL,

    CONSTRAINT pk_college_change_log PRIMARY KEY (id),
    CONSTRAINT fk_college_change_log_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_college_change_log_college_id (college_id),
);

EXEC sp_AddExtendedProperty 'MS_Description', N'院校更新记录', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college_change_log';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college_change_log', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'院校', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college_change_log', 'COLUMN', 'college_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新描述', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college_change_log', 'COLUMN', 'change_desc';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college_change_log', 'COLUMN', 'create_time';

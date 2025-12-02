CREATE TABLE tbl_action_log
(
    id          BIGINT        NOT NULL,
    type_code   INT           NOT NULL, -- ActionLogType
    user_id     BIGINT        NOT NULL,

    action_desc NVARCHAR(512) NOT NULL CONSTRAINT df_action_log_desc DEFAULT '',

    create_time DATETIME      NOT NULL,

    CONSTRAINT pk_action_log PRIMARY KEY (id),
    CONSTRAINT fk_action_log_user_id FOREIGN KEY (user_id) REFERENCES tbl_user (id),
    INDEX ix_action_log_type_code (type_code),
);

EXEC sp_AddExtendedProperty 'MS_Description', N'操作日志', 'SCHEMA', 'dbo', 'TABLE', 'tbl_action_log';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_action_log', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'操作类型', 'SCHEMA', 'dbo', 'TABLE', 'tbl_action_log', 'COLUMN', 'type_code';
EXEC sp_AddExtendedProperty 'MS_Description', N'操作用户', 'SCHEMA', 'dbo', 'TABLE', 'tbl_action_log', 'COLUMN', 'user_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'操作描述', 'SCHEMA', 'dbo', 'TABLE', 'tbl_action_log', 'COLUMN', 'action_desc';

EXEC sp_AddExtendedProperty 'MS_Description', N'操作时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_action_log', 'COLUMN', 'create_time';

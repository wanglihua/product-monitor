CREATE TABLE tbl_web_session
(
    id           BIGINT       NOT NULL,
    session_id   NVARCHAR(64) NOT NULL, -- 44长度就够了的
    update_count INT          NOT NULL,

    create_time  DATETIME     NOT NULL,
    update_time  DATETIME     NOT NULL,

    CONSTRAINT pk_web_session PRIMARY KEY (id),
);

CREATE UNIQUE INDEX ix_web_session_session_id ON tbl_web_session (session_id);

EXEC sp_AddExtendedProperty 'MS_Description', N'Web Session', 'SCHEMA', 'dbo', 'TABLE', 'tbl_web_session';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_web_session', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'Session Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_web_session', 'COLUMN', 'session_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新次数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_web_session', 'COLUMN', 'update_count';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_web_session', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_web_session', 'COLUMN', 'update_time';
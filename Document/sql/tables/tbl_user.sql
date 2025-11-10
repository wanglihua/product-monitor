CREATE TABLE tbl_user
(
    id              BIGINT        NOT NULL,

    login_name      NVARCHAR(128) NOT NULL,
    user_name       NVARCHAR(128) NOT NULL,
    password        NVARCHAR(128) NOT NULL,
    salt            NVARCHAR(128) NOT NULL,
    last_login_time DATETIME      NULL,

    status_id       INT           NOT NULL,

    create_time     DATETIME      NOT NULL,
    update_time     DATETIME      NOT NULL,

    CONSTRAINT pk_user PRIMARY KEY (id),
    INDEX ix_user_login_name (login_name)
);

-- todo 再加一个状态字段

-- 密码 123456
INSERT INTO tbl_user (id, login_name, user_name, password, salt, last_login_time, create_time, update_time)
VALUES (1, 'admin', N'管理员', '3e9122575ea7e252f3a3c104d534c4f17fd6a720', '550bea8abc', NULL, '2025-10-01', '2025-10-01');


EXEC sp_AddExtendedProperty 'MS_Description', N'用户', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'登录名', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user', 'COLUMN', 'login_name';
EXEC sp_AddExtendedProperty 'MS_Description', N'用户名', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user', 'COLUMN', 'user_name';
EXEC sp_AddExtendedProperty 'MS_Description', N'密码', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user', 'COLUMN', 'password';
EXEC sp_AddExtendedProperty 'MS_Description', N'盐', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user', 'COLUMN', 'salt';
EXEC sp_AddExtendedProperty 'MS_Description', N'最近登录时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user', 'COLUMN', 'last_login_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'状态', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user', 'COLUMN', 'status_id';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user', 'COLUMN', 'update_time';
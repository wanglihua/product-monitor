CREATE TABLE tbl_admin_user
(
    id          BIGINT   NOT NULL,

    user_id     BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,

    CONSTRAINT pk_admin_user PRIMARY KEY (id),
    CONSTRAINT fk_admin_user_user_id FOREIGN KEY (user_id) REFERENCES tbl_user (id),
    INDEX ix_admin_user_user_id (user_id),
);

INSERT INTO tbl_admin_user(id, user_id, create_time)
VALUES (1, 1, '2025-10-01');

EXEC sp_AddExtendedProperty 'MS_Description', N'管理员', 'SCHEMA', 'dbo', 'TABLE', 'tbl_admin_user';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_admin_user', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'用户', 'SCHEMA', 'dbo', 'TABLE', 'tbl_admin_user', 'COLUMN', 'user_id';

EXEC sp_AddExtendedProperty 'MS_Description', N'添加时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_admin_user', 'COLUMN', 'create_time';

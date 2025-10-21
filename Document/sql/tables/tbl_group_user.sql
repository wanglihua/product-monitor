CREATE TABLE tbl_group_user
(
    id          BIGINT   NOT NULL,

    group_id    BIGINT   NOT NULL,
    user_id     BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,

    CONSTRAINT pk_group_user PRIMARY KEY (id),
    INDEX ix_group_user_group_id (group_id),
    INDEX ix_group_user_user_id (user_id)
);

EXEC sp_AddExtendedProperty 'MS_Description', N'群组成员', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group_user';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group_user', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'群组', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group_user', 'COLUMN', 'group_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'用户', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group_user', 'COLUMN', 'user_id';

EXEC sp_AddExtendedProperty 'MS_Description', N'添加时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group_user', 'COLUMN', 'create_time';

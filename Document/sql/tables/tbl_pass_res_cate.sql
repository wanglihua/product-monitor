CREATE TABLE tbl_pass_res_cate
(
    id          BIGINT        NOT NULL,

    name        NVARCHAR(128) NOT NULL,

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_pass_res_cate PRIMARY KEY (id),
);

EXEC sp_AddExtendedProperty 'MS_Description', N'授权资源类别', 'SCHEMA', 'dbo', 'TABLE', 'tbl_pass_res_cate';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_pass_res_cate', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'类别名称', 'SCHEMA', 'dbo', 'TABLE', 'tbl_pass_res_cate', 'COLUMN', 'name';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_pass_res_cate', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_pass_res_cate', 'COLUMN', 'update_time';
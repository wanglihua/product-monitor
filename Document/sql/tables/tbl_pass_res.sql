CREATE TABLE tbl_pass_res
(
    id          BIGINT        NOT NULL,
    cate_id     BIGINT        NOT NULL,

    name        NVARCHAR(128) NOT NULL,

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_pass_res PRIMARY KEY (id),
    CONSTRAINT fk_pass_res_cate_id FOREIGN KEY (cate_id) REFERENCES tbl_pass_res_cate (id)
);

EXEC sp_AddExtendedProperty 'MS_Description', N'授权码资源', 'SCHEMA', 'dbo', 'TABLE', 'tbl_pass_res';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_pass_res', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'资源类别', 'SCHEMA', 'dbo', 'TABLE', 'tbl_pass_res', 'COLUMN', 'cate_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'资源名称', 'SCHEMA', 'dbo', 'TABLE', 'tbl_pass_res', 'COLUMN', 'name';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_pass_res', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_pass_res', 'COLUMN', 'update_time';
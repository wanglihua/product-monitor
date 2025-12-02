CREATE TABLE tbl_group_pass_res
(
    id          BIGINT   NOT NULL,

    group_id    BIGINT   NOT NULL,
    pass_res_id BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,

    CONSTRAINT pk_group_pass_res PRIMARY KEY (id),
    CONSTRAINT fk_group_pass_res_group_id FOREIGN KEY (group_id) REFERENCES tbl_group (id),
    INDEX ix_group_pass_res_group_id (group_id),
    CONSTRAINT fk_group_pass_res_pass_res_id FOREIGN KEY (pass_res_id) REFERENCES tbl_pass_res (id),
    INDEX ix_group_pass_res_pass_res_id (pass_res_id),
);

EXEC sp_AddExtendedProperty 'MS_Description', N'群组授权', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group_pass_res';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group_pass_res', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'群组', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group_pass_res', 'COLUMN', 'group_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'资源', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group_pass_res', 'COLUMN', 'pass_res_id';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_group_pass_res', 'COLUMN', 'create_time';

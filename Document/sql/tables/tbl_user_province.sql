CREATE TABLE tbl_user_province
(
    id            BIGINT   NOT NULL,

    user_id       BIGINT   NOT NULL,
    province_type INT      NOT NULL,

    create_time   DATETIME NOT NULL,

    CONSTRAINT pk_user_province PRIMARY KEY (id),
    CONSTRAINT fk_user_province_user_id FOREIGN KEY (user_id) REFERENCES tbl_user (id),
    INDEX ix_user_province_user_id (user_id),
);


EXEC sp_AddExtendedProperty 'MS_Description', N'', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user_province';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user_province', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'用户', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user_province', 'COLUMN', 'user_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'省份', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user_province', 'COLUMN', 'province_type';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user_province', 'COLUMN', 'create_time';

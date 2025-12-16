CREATE TABLE tbl_product_url_monitor_entry
(
    id                     BIGINT        NOT NULL,

    product_url_monitor_id BIGINT        NOT NULL,

    login_user             NVARCHAR(128) NOT NULL,
    account_type           INT           NOT NULL CONSTRAINT df_product_url_monitor_entry_account_type DEFAULT 0,
    ip_address             NVARCHAR(32)  NOT NULL,
    province_type          INT           NOT NULL CONSTRAINT df_product_url_monitor_entry_province_type DEFAULT 0,
    province               NVARCHAR(20)  NOT NULL CONSTRAINT df_product_install_monitor_entry_province DEFAULT '',

    create_time            DATETIME      NOT NULL,
    update_time            DATETIME      NOT NULL,

    CONSTRAINT pk_product_url_monitor_entry PRIMARY KEY (id),
    CONSTRAINT fk_product_url_monitor_entry_pumi FOREIGN KEY (product_url_monitor_id) REFERENCES tbl_product_url_monitor (id),
);

EXEC sp_AddExtendedProperty 'MS_Description', N'网站监测详情', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'网站监测', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry', 'COLUMN', 'product_url_monitor_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'账号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry', 'COLUMN', 'login_user';
EXEC sp_AddExtendedProperty 'MS_Description', N'账号分类', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry', 'COLUMN', 'account_type';
EXEC sp_AddExtendedProperty 'MS_Description', N'使用地址', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry', 'COLUMN', 'ip_address';
EXEC sp_AddExtendedProperty 'MS_Description', N'省份', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry', 'COLUMN', 'province_type';
EXEC sp_AddExtendedProperty 'MS_Description', N'省份', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry', 'COLUMN', 'province';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry', 'COLUMN', 'update_time';


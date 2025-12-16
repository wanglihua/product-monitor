ALTER TABLE tbl_product_install_monitor_entry
    ADD province NVARCHAR(20) NOT NULL CONSTRAINT df_product_install_monitor_entry_province DEFAULT '';

EXEC sp_AddExtendedProperty 'MS_Description', N'使用省份', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor_entry', 'COLUMN', 'province';

ALTER TABLE tbl_product_url_monitor_entry
    ADD province NVARCHAR(20) NOT NULL CONSTRAINT df_product_url_monitor_entry_province DEFAULT '';

EXEC sp_AddExtendedProperty 'MS_Description', N'省份', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry', 'COLUMN', 'province';

DROP INDEX ix_user_login_name ON tbl_user;

CREATE UNIQUE INDEX ix_user_login_name ON tbl_user (login_name);

ALTER TABLE tbl_college_change_log
    ADD former_name NVARCHAR(128) NOT NULL CONSTRAINT df_college_change_log_name DEFAULT '';

ALTER TABLE tbl_college_change_log
    ADD update_time DATETIME NOT NULL;

EXEC sp_AddExtendedProperty 'MS_Description', N'曾用名', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college_change_log', 'COLUMN', 'former_name';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_college_change_log', 'COLUMN', 'update_time';


CREATE UNIQUE INDEX ix_college_change_log_former_name ON tbl_college_change_log (former_name);

CREATE UNIQUE INDEX ix_group_name ON tbl_group (name);

DROP INDEX ix_product_name ON tbl_product;
CREATE UNIQUE INDEX ix_product_name ON tbl_product (name);

-- DROP INDEX ix_college_code ON tbl_college;
-- CREATE UNIQUE INDEX ix_college_code ON tbl_college (code);
CREATE UNIQUE INDEX ix_college_name ON tbl_college (name);
ALTER TABLE tbl_product_install_monitor_entry
    ADD province NVARCHAR(20) NOT NULL CONSTRAINT df_product_install_monitor_entry_province DEFAULT '';

EXEC sp_AddExtendedProperty 'MS_Description', N'使用省份', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor_entry', 'COLUMN', 'province';

ALTER TABLE tbl_product_url_monitor_entry
    ADD province NVARCHAR(20) NOT NULL CONSTRAINT df_product_url_monitor_entry_province DEFAULT '';

EXEC sp_AddExtendedProperty 'MS_Description', N'省份', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor_entry', 'COLUMN', 'province';


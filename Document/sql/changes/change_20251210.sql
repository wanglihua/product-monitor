ALTER TABLE tbl_product_install_monitor_entry
    ADD use_time datetime NOT NULL CONSTRAINT df_product_install_monitor_entry_use_time DEFAULT GETDATE();

EXEC sp_AddExtendedProperty 'MS_Description', N'使用时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor_entry', 'COLUMN', 'use_time';
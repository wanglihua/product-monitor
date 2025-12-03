CREATE INDEX ix_product_name ON tbl_product (name);

ALTER TABLE tbl_product_install_monitor_entry
    ADD cpu_serial_no NVARCHAR(128) NOT NULL CONSTRAINT df_product_install_monitor_entry_cpu_serial_no DEFAULT '';

ALTER TABLE tbl_product_install_monitor_entry
    ADD net_mac_address NVARCHAR(128) NOT NULL CONSTRAINT df_product_install_monitor_entry_net_mac_address DEFAULT '';

EXEC sp_AddExtendedProperty 'MS_Description', N'CPU序列号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor_entry', 'COLUMN', 'cpu_serial_no';
EXEC sp_AddExtendedProperty 'MS_Description', N'网卡硬件地址', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor_entry', 'COLUMN', 'net_mac_address';


ALTER TABLE tbl_product_special_service
    ADD project_type_code INT NOT NULL CONSTRAINT df_product_special_service_project_type DEFAULT 0;

EXEC sp_AddExtendedProperty 'MS_Description', N'项目类型', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_special_service', 'COLUMN', 'project_type_code';

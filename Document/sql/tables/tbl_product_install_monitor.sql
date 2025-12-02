CREATE TABLE tbl_product_install_monitor
(
    id              BIGINT        NOT NULL,

    college_id      BIGINT        NOT NULL,
    product_id      BIGINT        NOT NULL,

    province_type   INT           NOT NULL CONSTRAINT df_product_install_monitor_province_type DEFAULT 0,

    cpu_serial_no   NVARCHAR(128) NOT NULL,
    net_mac_address NVARCHAR(128) NOT NULL,

    task_count      INT           NOT NULL,
    teacher_count   INT           NOT NULL,

    create_time     DATETIME      NOT NULL,
    update_time     DATETIME      NOT NULL,

    CONSTRAINT pk_product_install_monitor PRIMARY KEY (id),
    CONSTRAINT fk_product_install_monitor_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    CONSTRAINT fk_product_install_monitor_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
);

EXEC sp_AddExtendedProperty 'MS_Description', N'安装监测', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'学校', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor', 'COLUMN', 'college_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'省份', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor', 'COLUMN', 'province_type';
EXEC sp_AddExtendedProperty 'MS_Description', N'CPU序列号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor', 'COLUMN', 'cpu_serial_no';
EXEC sp_AddExtendedProperty 'MS_Description', N'网卡硬件地址', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor', 'COLUMN', 'net_mac_address';
EXEC sp_AddExtendedProperty 'MS_Description', N'任务数量', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor', 'COLUMN', 'task_count';
EXEC sp_AddExtendedProperty 'MS_Description', N'教师数量', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor', 'COLUMN', 'teacher_count';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor', 'COLUMN', 'update_time';
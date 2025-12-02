CREATE TABLE tbl_product_url_monitor
(
    id               BIGINT   NOT NULL,

    college_id       BIGINT   NOT NULL,
    product_id       BIGINT   NOT NULL,

    product_url_type INT      NOT NULL CONSTRAINT df_product_url_monitor_product_url_type DEFAULT 0,

    task_count       INT      NOT NULL,
    teacher_count    INT      NOT NULL,

    create_time      DATETIME NOT NULL,
    update_time      DATETIME NOT NULL,

    CONSTRAINT pk_product_url_monitor PRIMARY KEY (id),
    CONSTRAINT fk_product_url_monitor_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    CONSTRAINT fk_product_url_monitor_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
);

EXEC sp_AddExtendedProperty 'MS_Description', N'网站监测', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'学校', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor', 'COLUMN', 'college_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'分类', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor', 'COLUMN', 'product_url_type';
EXEC sp_AddExtendedProperty 'MS_Description', N'任务数量', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor', 'COLUMN', 'task_count';
EXEC sp_AddExtendedProperty 'MS_Description', N'教师数量', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor', 'COLUMN', 'teacher_count';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url_monitor', 'COLUMN', 'update_time';
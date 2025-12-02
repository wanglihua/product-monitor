CREATE TABLE tbl_product_eval_entry
(
    id              BIGINT        NOT NULL,
    product_eval_id BIGINT        NOT NULL,

    total_days      INT           NOT NULL,

    begin_date      DATETIME      NOT NULL,
    end_date        DATETIME      NOT NULL,

    sales_manager   NVARCHAR(32)  NOT NULL,
    contact_teacher NVARCHAR(32)  NOT NULL,
    contact_mobile  NVARCHAR(32)  NOT NULL,

    description     NVARCHAR(512) NOT NULL,

    user_id         BIGINT        NOT NULL,

    create_time     DATETIME      NOT NULL,
    update_time     DATETIME      NOT NULL,

    CONSTRAINT pk_product_eval_entry PRIMARY KEY (id),

    CONSTRAINT fk_product_eval_entry_product_eval_id FOREIGN KEY (product_eval_id) REFERENCES tbl_product_eval (id),
    INDEX ix_product_eval_entry_product_eval_id (product_eval_id),
);

-- DROP TABLE tbl_product_eval_entry;

EXEC sp_AddExtendedProperty 'MS_Description', N'试用目录明细', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'试用目录', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'product_eval_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'累计天数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'total_days';
EXEC sp_AddExtendedProperty 'MS_Description', N'开始时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'begin_date';
EXEC sp_AddExtendedProperty 'MS_Description', N'结束时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'end_date';
EXEC sp_AddExtendedProperty 'MS_Description', N'客户经理', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'sales_manager';
EXEC sp_AddExtendedProperty 'MS_Description', N'联系老师', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'contact_teacher';
EXEC sp_AddExtendedProperty 'MS_Description', N'联系电话', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'contact_mobile';
EXEC sp_AddExtendedProperty 'MS_Description', N'备注', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'description';
EXEC sp_AddExtendedProperty 'MS_Description', N'记录人', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'user_id';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_eval_entry', 'COLUMN', 'update_time';
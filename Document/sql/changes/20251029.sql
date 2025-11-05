DROP TABLE tbl_product_version;

CREATE TABLE tbl_product_version
(
    id                  BIGINT        NOT NULL,
    product_id          BIGINT        NOT NULL,

    version             NVARCHAR(32)  NOT NULL,
    version_update_base NVARCHAR(32)  NOT NULL,
    version_update_desc NVARCHAR(512) NOT NULL,
    version_update_time DATETIME      NOT NULL,

    create_time         DATETIME      NOT NULL,
    update_time         DATETIME      NOT NULL,

    CONSTRAINT pk_product_version PRIMARY KEY (id),
    CONSTRAINT fk_product_version_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_version_product_id (product_id)
);

EXEC sp_AddExtendedProperty 'MS_Description', N'产品版本', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'产品目录', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'版本号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'version';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新基础', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'version_update_base';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新内容', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'version_update_desc';
EXEC sp_AddExtendedProperty 'MS_Description', N'版本更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'version_update_time';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_version', 'COLUMN', 'update_time';

GO

UPDATE tbl_college
SET province = '0'
WHERE id > 0;

ALTER TABLE tbl_college
    ALTER COLUMN province INT NOT NULL

ALTER TABLE tbl_college
    ADD DEFAULT 0 FOR province;

GO

UPDATE tbl_college
SET level = '0'
WHERE id > 0;

ALTER TABLE tbl_college
    ALTER COLUMN level INT NOT NULL;

ALTER TABLE tbl_college
    ADD DEFAULT 0 FOR level;

GO

ALTER TABLE tbl_product
    DROP COLUMN version;

ALTER TABLE tbl_product
    DROP COLUMN version_update_base;

ALTER TABLE tbl_product
    DROP COLUMN version_update_desc;
GO

ALTER TABLE tbl_product
    ADD english_name NVARCHAR(256) NOT NULL DEFAULT '';

ALTER TABLE tbl_product
    ADD version_count INT NOT NULL DEFAULT 0;

ALTER TABLE tbl_product
    ADD latest_version NVARCHAR(32) NOT NULL DEFAULT '';

EXEC sp_AddExtendedProperty 'MS_Description', N'产品英文名', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product', 'COLUMN', 'english_name';
EXEC sp_AddExtendedProperty 'MS_Description', N'版本数量', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product', 'COLUMN', 'version_count';
EXEC sp_AddExtendedProperty 'MS_Description', N'最新版本号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product', 'COLUMN', 'latest_version';

ALTER TABLE tbl_product
    ALTER COLUMN version_update_time DATETIME NULL;

GO

ALTER TABLE tbl_product_sale_entry
    ADD sales_manager NVARCHAR(32) NOT NULL DEFAULT '';

ALTER TABLE tbl_product_sale_entry
    ADD contact_teacher NVARCHAR(32) NOT NULL DEFAULT '';

ALTER TABLE tbl_product_sale_entry
    ADD contact_mobile NVARCHAR(32) NOT NULL DEFAULT '';

ALTER TABLE tbl_product_sale_entry
    ADD user_id BIGINT NOT NULL DEFAULT 1;

EXEC sp_AddExtendedProperty 'MS_Description', N'客户经理', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'sales_manager';
EXEC sp_AddExtendedProperty 'MS_Description', N'联系老师', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'contact_teacher';
EXEC sp_AddExtendedProperty 'MS_Description', N'联系电话', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'contact_mobile';
EXEC sp_AddExtendedProperty 'MS_Description', N'记录人', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_sale_entry', 'COLUMN', 'user_id';

GO


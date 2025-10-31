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


ALTER TABLE tbl_product_install_monitor_entry
    ADD use_time DATETIME NOT NULL CONSTRAINT df_product_install_monitor_entry_use_time DEFAULT GETDATE();

EXEC sp_AddExtendedProperty 'MS_Description', N'使用时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_monitor_entry', 'COLUMN', 'use_time';

ALTER TABLE tbl_product_url
    DROP COLUMN name;

-- 这个运行时需要删除 tbl_product_url 中的所有数据
--TRUNCATE TABLE tbl_product_url;

ALTER TABLE tbl_product_url
    ADD product_id BIGINT NOT NULL;

ALTER TABLE tbl_product_url
    ADD CONSTRAINT fk_product_url_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id);

CREATE INDEX ix_product_url_product_id ON tbl_product_url (product_id);

EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_url', 'COLUMN', 'product_id';

CREATE TABLE tbl_user_province
(
    id            BIGINT   NOT NULL,

    user_id       BIGINT   NOT NULL,
    province_type INT      NOT NULL,

    create_time   DATETIME NOT NULL,

    CONSTRAINT pk_user_province PRIMARY KEY (id),
    CONSTRAINT fk_user_province_user_id FOREIGN KEY (user_id) REFERENCES tbl_user (id)
);

CREATE INDEX ix_user_province_user_id ON tbl_user_province (user_id);


EXEC sp_AddExtendedProperty 'MS_Description', N'', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user_province';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user_province', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user_province', 'COLUMN', 'user_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user_province', 'COLUMN', 'province_type';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_user_province', 'COLUMN', 'create_time';
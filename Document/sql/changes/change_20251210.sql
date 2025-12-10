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


CREATE TABLE tbl_client_key
(
    id          BIGINT        NOT NULL,

    college_id  BIGINT        NOT NULL,
    product_id  BIGINT        NOT NULL,

    key_type    INT           NOT NULL CONSTRAINT df_client_key_key_type DEFAULT 0,

    client_key  NVARCHAR(128) NOT NULL,

    user_id     BIGINT        NOT NULL,

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_client_key PRIMARY KEY (id),
    CONSTRAINT fk_client_key_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    CONSTRAINT fk_client_key_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    CONSTRAINT fk_client_key_user_id FOREIGN KEY (user_id) REFERENCES tbl_user (id)
);

CREATE INDEX ix_client_key_college_id ON tbl_client_key (college_id);
CREATE INDEX ix_client_key_product_id ON tbl_client_key(product_id);
CREATE INDEX ix_client_key_key ON tbl_client_key(client_key);

EXEC sp_AddExtendedProperty 'MS_Description', N'客户端密钥', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'学校', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'college_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'类型', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'key_type';
EXEC sp_AddExtendedProperty 'MS_Description', N'密钥', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'client_key';
EXEC sp_AddExtendedProperty 'MS_Description', N'操作人', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'user_id';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_client_key', 'COLUMN', 'update_time';
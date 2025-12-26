CREATE TABLE tbl_product_install_attachment
(
    id                 BIGINT        NOT NULL,
    product_install_id BIGINT        NOT NULL,

    name               NVARCHAR(200) NOT NULL,
    path_file          NVARCHAR(300) NOT NULL,

    create_time        DATETIME      NOT NULL,

    CONSTRAINT pk_product_install_attachment PRIMARY KEY (id),
    CONSTRAINT fk_product_install_attachment_product_install_id FOREIGN KEY (product_install_id) REFERENCES tbl_product_install (id)
);

EXEC sp_AddExtendedProperty 'MS_Description', N'', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_attachment';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_attachment', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'产品安装', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_attachment', 'COLUMN', 'product_install_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'附件名', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_attachment', 'COLUMN', 'name';
EXEC sp_AddExtendedProperty 'MS_Description', N'文件路径', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_attachment', 'COLUMN', 'path_file';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install_attachment', 'COLUMN', 'create_time';

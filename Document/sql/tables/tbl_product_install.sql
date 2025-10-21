CREATE TABLE tbl_product_install
(
    id              BIGINT        NOT NULL,

    college_id      BIGINT        NOT NULL,
    product_id      BIGINT        NOT NULL,

    version_sale    NVARCHAR(32)  NOT NULL,
    version_install NVARCHAR(32)  NOT NULL,
    install_date    DATETIME      NOT NULL,
    update_url      NVARCHAR(256) NOT NULL,
    login_user      NVARCHAR(128) NOT NULL,
    login_password  NVARCHAR(128) NOT NULL,
    contact_teacher NVARCHAR(32)  NOT NULL,
    contact_mobile  NVARCHAR(32)  NOT NULL,

    description     NVARCHAR(512) NOT NULL,

    create_time     DATETIME      NOT NULL,
    update_time     DATETIME      NOT NULL,

    CONSTRAINT pk_product_install PRIMARY KEY (id),
    CONSTRAINT fk_product_install_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_product_install_college_id (college_id),
    CONSTRAINT fk_product_install_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_install_product_id (product_id),
);

EXEC sp_AddExtendedProperty 'MS_Description', N'产品安装', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install';
EXEC sp_AddExtendedProperty 'MS_Description', N'学校', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install', 'COLUMN', 'college_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'销售版本', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install', 'COLUMN', 'version_sale';
EXEC sp_AddExtendedProperty 'MS_Description', N'安装版本', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install', 'COLUMN', 'version_install';
EXEC sp_AddExtendedProperty 'MS_Description', N'安装时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install', 'COLUMN', 'install_date';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新网址', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install', 'COLUMN', 'update_url';
EXEC sp_AddExtendedProperty 'MS_Description', N'系统账号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_install', 'COLUMN', 'login_user';

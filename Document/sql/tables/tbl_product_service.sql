CREATE TABLE tbl_product_service
(
    id                BIGINT        NOT NULL,

    college_id        BIGINT        NOT NULL,
    product_id        BIGINT        NOT NULL,

    project_type_code INT           NOT NULL,
    service_date      DATETIME      NOT NULL,
    service_count     INT           NOT NULL,
    service_url       NVARCHAR(256) NOT NULL,
    login_user        NVARCHAR(128) NOT NULL,
    login_password    NVARCHAR(128) NOT NULL,
    contact_teacher   NVARCHAR(32)  NOT NULL,
    contact_mobile    NVARCHAR(32)  NOT NULL,

    description       NVARCHAR(512) NOT NULL,

    create_time       DATETIME      NOT NULL,
    update_time       DATETIME      NOT NULL,

    CONSTRAINT pk_product_service PRIMARY KEY (id),
    CONSTRAINT fk_product_service_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_product_service_college_id (college_id),
    CONSTRAINT fk_product_service_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_service_product_id (product_id)
);


EXEC sp_AddExtendedProperty 'MS_Description', N'售后服务', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service';

EXEC sp_AddExtendedProperty 'MS_Description', N'Id', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'id';

EXEC sp_AddExtendedProperty 'MS_Description', N'学校', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'college_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'产品', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'product_id';
EXEC sp_AddExtendedProperty 'MS_Description', N'项目类型', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'project_type_code';
EXEC sp_AddExtendedProperty 'MS_Description', N'售后时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'service_date';
EXEC sp_AddExtendedProperty 'MS_Description', N'售后次数', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'service_count';
EXEC sp_AddExtendedProperty 'MS_Description', N'售后网址', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'service_url';
EXEC sp_AddExtendedProperty 'MS_Description', N'售后账号', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'login_user';
EXEC sp_AddExtendedProperty 'MS_Description', N'售后密码', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'login_password';
EXEC sp_AddExtendedProperty 'MS_Description', N'联系老师', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'contact_teacher';
EXEC sp_AddExtendedProperty 'MS_Description', N'联系电话', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'contact_mobile';
EXEC sp_AddExtendedProperty 'MS_Description', N'售后问题', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'description';

EXEC sp_AddExtendedProperty 'MS_Description', N'创建时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'create_time';
EXEC sp_AddExtendedProperty 'MS_Description', N'更新时间', 'SCHEMA', 'dbo', 'TABLE', 'tbl_product_service', 'COLUMN', 'update_time';
CREATE TABLE tbl_product_update
(
    id              BIGINT        NOT NULL,

    college_id      BIGINT        NOT NULL,
    product_id      BIGINT        NOT NULL,

    version_sale    NVARCHAR(32)  NOT NULL,
    version_update  NVARCHAR(32)  NOT NULL,
    update_count    INT           NOT NULL,
    update_url      NVARCHAR(256) NOT NULL,
    login_user      NVARCHAR(128) NOT NULL,
    login_password  NVARCHAR(128) NOT NULL,
    update_date     DATETIME      NOT NULL,
    sales_manager   NVARCHAR(32)  NOT NULL,
    contact_teacher NVARCHAR(32)  NOT NULL,
    contact_mobile  NVARCHAR(32)  NOT NULL,

    description     NVARCHAR(512) NOT NULL,

    create_time     DATETIME      NOT NULL,
    update_time     DATETIME      NOT NULL,

    CONSTRAINT pk_product_update PRIMARY KEY (id),
    CONSTRAINT fk_product_update_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_product_update_college_id (college_id),
    CONSTRAINT fk_product_update_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_update_product_id (product_id)
);


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
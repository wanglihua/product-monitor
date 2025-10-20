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
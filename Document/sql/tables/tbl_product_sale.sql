CREATE TABLE tbl_product_sale
(
    id                  BIGINT        NOT NULL,
    college_id          BIGINT        NOT NULL,
    product_id          BIGINT        NOT NULL,

    sale_times          INT           NOT NULL,
    install_times       INT           NOT NULL,
    update_times        INT           NOT NULL,
    service_times       INT           NOT NULL,

    version             NVARCHAR(32)  NOT NULL,

    sale_date           DATETIME      NOT NULL,
    service_expire_date DATETIME      NOT NULL,

    description         NVARCHAR(512) NOT NULL,

    create_time         DATETIME      NOT NULL,
    update_time         DATETIME      NOT NULL,

    CONSTRAINT pk_product_sale PRIMARY KEY (id),
    CONSTRAINT fk_product_sale_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_sale_product_id (product_id),
    CONSTRAINT fk_product_sale_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_college_sale_college_id (college_id),
);

-- DROP TABLE tbl_product_sale;
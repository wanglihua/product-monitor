CREATE TABLE tbl_product_special_service
(
    id          BIGINT   NOT NULL,

    college_id  BIGINT   NOT NULL,
    product_id  BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,
    update_time DATETIME NOT NULL,

    CONSTRAINT pk_product_special_service PRIMARY KEY (id),
    CONSTRAINT fk_product_special_service_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_product_special_service_college_id (college_id),
    CONSTRAINT fk_product_special_service_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_special_service_product_id (product_id)
);
CREATE TABLE tbl_product_sale
(
    id          BIGINT   NOT NULL,
    product_id  BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,
    update_time DATETIME NOT NULL,

    CONSTRAINT pk_product_sale PRIMARY KEY (id),
    CONSTRAINT fk_product_sale_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_sale_product_id (product_id)
);
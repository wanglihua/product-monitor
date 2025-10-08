CREATE TABLE tbl_product_sale_entry
(
    id              BIGINT   NOT NULL,
    product_sale_id BIGINT   NOT NULL,

    create_time     DATETIME NOT NULL,
    update_time     DATETIME NOT NULL,

    CONSTRAINT pk_product_sale_entry PRIMARY KEY (id),

    CONSTRAINT fk_product_sale_entry_product_sale_id FOREIGN KEY (product_sale_id) REFERENCES tbl_product_sale (id),
    INDEX ix_product_sale_entry_product_sale_id (product_sale_id),
);
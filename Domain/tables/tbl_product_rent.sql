CREATE TABLE tbl_product_rent
(
    id          BIGINT   NOT NULL,
    product_id  BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,
    update_time DATETIME NOT NULL,

    CONSTRAINT pk_product_rent PRIMARY KEY (id),
    CONSTRAINT fk_product_rent_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_rent_product_id (product_id)
);
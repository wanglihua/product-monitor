CREATE TABLE tbl_product_rent_entry
(
    id              BIGINT   NOT NULL,
    product_rent_id BIGINT   NOT NULL,

    create_time     DATETIME NOT NULL,
    update_time     DATETIME NOT NULL,

    CONSTRAINT pk_product_rent_entry PRIMARY KEY (id),

    CONSTRAINT fk_product_rent_entry_product_rent_id FOREIGN KEY (product_rent_id) REFERENCES tbl_product_rent (id),
    INDEX ix_product_rent_entry_product_rent_id (product_rent_id),

);
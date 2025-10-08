CREATE TABLE tbl_product_eval
(
    id          BIGINT   NOT NULL,
    product_id  BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,
    update_time DATETIME NOT NULL,

    CONSTRAINT pk_product_eval PRIMARY KEY (id),
    CONSTRAINT fk_product_eval_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_eval_product_id (product_id)
);
CREATE TABLE tbl_product_update
(
    id          BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,
    update_time DATETIME NOT NULL,

    CONSTRAINT pk_product_update PRIMARY KEY (id)
);
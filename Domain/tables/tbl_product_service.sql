CREATE TABLE tbl_product_service
(
    id          BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,
    update_time DATETIME NOT NULL,

    CONSTRAINT pk_product_service PRIMARY KEY (id)
);
CREATE TABLE tbl_product_install
(
    id          BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,
    update_time DATETIME NOT NULL,

    CONSTRAINT pk_product_install PRIMARY KEY (id)
);
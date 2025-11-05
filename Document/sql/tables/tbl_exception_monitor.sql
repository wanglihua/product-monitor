CREATE TABLE tbl_exception_monitor
(
    id            BIGINT       NOT NULL,

    product_id    BIGINT       NOT NULL,

    ip_address    NVARCHAR(32) NOT NULL,
    province_type INT          NOT NULL DEFAULT 0,
    port          INT          NOT NULL,

    create_time   DATETIME     NOT NULL,
    update_time   DATETIME     NOT NULL,

    CONSTRAINT pk_exception_monitor PRIMARY KEY (id)
);
CREATE TABLE tbl_product_url_monitor_entry
(
    id                     BIGINT        NOT NULL,

    product_url_monitor_id BIGINT        NOT NULL,

    login_user             NVARCHAR(128) NOT NULL,
    login_password         NVARCHAR(128) NOT NULL,
    ip_address             NVARCHAR(32)  NOT NULL,
    province_type          INT           NOT NULL DEFAULT 0,

    create_time            DATETIME      NOT NULL,
    update_time            DATETIME      NOT NULL,

    CONSTRAINT pk_product_url_monitor_entry PRIMARY KEY (id),
    CONSTRAINT fk_product_url_monitor_entry_pumi FOREIGN KEY (product_url_monitor_id) REFERENCES tbl_product_url_monitor (id),
);
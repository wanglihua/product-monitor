CREATE TABLE tbl_product_install_monitor_entry
(
    id                 BIGINT        NOT NULL,

    product_monitor_id BIGINT        NOT NULL,

    login_user         NVARCHAR(128) NOT NULL,
    account_type       INT           NOT NULL DEFAULT 0,
    ip_address         NVARCHAR(32)  NOT NULL,
    province_type      INT           NOT NULL,

    create_time        DATETIME      NOT NULL,
    update_time        DATETIME      NOT NULL,

    CONSTRAINT pk_product_install_monitor_entry PRIMARY KEY (id),
    CONSTRAINT fk_product_install_monitor_entry_pmi FOREIGN KEY (product_monitor_id) REFERENCES tbl_product_install_monitor (id),
);

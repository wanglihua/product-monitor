CREATE TABLE tbl_product_install_monitor
(
    id              BIGINT        NOT NULL,

    college_id      BIGINT        NOT NULL,
    product_id      BIGINT        NOT NULL,

    province_type   INT           NOT NULL DEFAULT 0,

    cpu_serial_no   NVARCHAR(128) NOT NULL,
    net_mac_address NVARCHAR(128) NOT NULL,

    task_count      INT           NOT NULL,
    teacher_count   INT           NOT NULL,

    create_time     DATETIME      NOT NULL,
    update_time     DATETIME      NOT NULL,

    CONSTRAINT pk_product_install_monitor PRIMARY KEY (id),
);
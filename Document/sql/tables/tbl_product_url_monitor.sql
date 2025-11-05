CREATE TABLE tbl_product_url_monitor
(
    id               BIGINT   NOT NULL,

    college_id       BIGINT   NOT NULL,
    product_id       BIGINT   NOT NULL,

    product_url_type INT      NOT NULL DEFAULT 0,

    task_count       INT      NOT NULL,
    teacher_count    INT      NOT NULL,

    create_time      DATETIME NOT NULL,
    update_time      DATETIME NOT NULL,

    CONSTRAINT pk_product_url_monitor PRIMARY KEY (id),
    CONSTRAINT fk_product_url_monitor_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    CONSTRAINT fk_product_url_monitor_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
);
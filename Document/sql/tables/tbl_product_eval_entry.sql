CREATE TABLE tbl_product_eval_entry
(
    id              BIGINT        NOT NULL,
    product_eval_id BIGINT        NOT NULL,

    total_days      INT           NOT NULL,

    begin_date      DATETIME      NOT NULL,
    end_date        DATETIME      NOT NULL,

    description     NVARCHAR(512) NOT NULL,

    create_time     DATETIME      NOT NULL,
    update_time     DATETIME      NOT NULL,

    CONSTRAINT pk_product_eval_entry PRIMARY KEY (id),

    CONSTRAINT fk_product_eval_entry_product_eval_id FOREIGN KEY (product_eval_id) REFERENCES tbl_product_eval (id),
    INDEX ix_product_eval_entry_product_eval_id (product_eval_id),

);

-- DROP TABLE tbl_product_eval_entry;
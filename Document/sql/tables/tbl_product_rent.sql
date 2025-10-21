CREATE TABLE tbl_product_rent
(
    id            BIGINT        NOT NULL,
    college_id    BIGINT        NOT NULL,
    product_id    BIGINT        NOT NULL,

    rent_times    INT           NOT NULL,
    service_times INT           NOT NULL,
    total_days    INT           NOT NULL,
    remain_days   INT           NOT NULL,

    begin_date    DATE          NOT NULL,
    end_date      DATE          NOT NULL,

    description   NVARCHAR(512) NOT NULL,

    create_time   DATETIME      NOT NULL,
    update_time   DATETIME      NOT NULL,

    CONSTRAINT pk_product_rent PRIMARY KEY (id),
    CONSTRAINT fk_product_rent_product_id FOREIGN KEY (product_id) REFERENCES tbl_product (id),
    INDEX ix_product_rent_product_id (product_id),
    CONSTRAINT fk_product_rent_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_college_rent_college_id (college_id),
);
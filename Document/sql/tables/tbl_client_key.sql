CREATE TABLE tbl_client_key
(
    id         bigint NOT NULL,

    college_id BIGINT NOT NULL,
    product_id BIGINT NOT NULL,

    key_type   INT    NOT NULL CONSTRAINT df_client_key_key_type DEFAULT 0,

    key        nvarchr(128) NOT NULL,

);yy
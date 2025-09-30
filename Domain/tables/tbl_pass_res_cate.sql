CREATE TABLE tbl_pass_res_cate
(
    id          BIGINT        NOT NULL,

    name        NVARCHAR(128) NOT NULL,

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_pass_res_cate PRIMARY KEY (id)
);
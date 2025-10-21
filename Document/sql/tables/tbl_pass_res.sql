CREATE TABLE tbl_pass_res
(
    id          BIGINT        NOT NULL,
    cate_id     BIGINT        NOT NULL,

    name        NVARCHAR(128) NOT NULL,

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_pass_res PRIMARY KEY (id),
    CONSTRAINT fk_pass_res_cate_id FOREIGN KEY (cate_id) REFERENCES tbl_pass_res_cate (id)
);
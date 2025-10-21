CREATE TABLE tbl_user_pass_res
(
    id          BIGINT   NOT NULL,

    user_id     BIGINT   NOT NULL,
    pass_res_id BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,

    CONSTRAINT pk_user_pass_res PRIMARY KEY (id),
    CONSTRAINT fk_user_pass_res_user_id FOREIGN KEY (user_id) REFERENCES tbl_user (id),
    INDEX ix_user_pass_res_user_id (user_id),
    CONSTRAINT fk_user_pass_res_pass_res_id FOREIGN KEY (pass_res_id) REFERENCES tbl_pass_res (id),
    INDEX ix_user_pass_res_pass_res_id (pass_res_id)
);
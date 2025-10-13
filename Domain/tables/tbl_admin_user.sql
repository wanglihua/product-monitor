CREATE TABLE tbl_admin_user
(
    id          BIGINT   NOT NULL,

    user_id     BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,

    CONSTRAINT pk_admin_user PRIMARY KEY (id),
    CONSTRAINT fk_admin_user_user_id FOREIGN KEY (user_id) REFERENCES tbl_user (id),
    INDEX ix_admin_user_user_id (user_id),
);

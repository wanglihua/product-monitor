CREATE TABLE tbl_group_user
(
    id          BIGINT   NOT NULL,

    group_id    BIGINT   NOT NULL,
    user_id     BIGINT   NOT NULL,

    create_time DATETIME NOT NULL,

    CONSTRAINT pk_group_user PRIMARY KEY (id),
    INDEX ix_group_user_group_id (group_id),
    INDEX ix_group_user_user_id (user_id)
);
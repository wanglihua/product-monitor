CREATE TABLE tbl_user
(
    id              BIGINT        NOT NULL,

    login_name      NVARCHAR(128) NOT NULL,
    user_name       NVARCHAR(128) NOT NULL,
    password        NVARCHAR(128) NOT NULL,
    salt            NVARCHAR(128) NOT NULL,
    last_login_time DATETIME      NULL,

    create_time     DATETIME      NOT NULL,
    update_time     DATETIME      NOT NULL,

    CONSTRAINT pk_user PRIMARY KEY (id),
    INDEX ix_user_login_name (login_name)
);

-- todo 再加一个状态字段

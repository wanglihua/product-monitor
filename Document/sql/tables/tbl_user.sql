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

-- 密码 123456
INSERT INTO tbl_user (id, login_name, user_name, password, salt, last_login_time, create_time, update_time)
VALUES (1, 'admin', N'管理员', '3e9122575ea7e252f3a3c104d534c4f17fd6a720', '550bea8abc', null, '2025-10-01', '2025-10-01');
CREATE TABLE tbl_college
(
    id          BIGINT        NOT NULL,

    code        NVARCHAR(64)  NOT NULL,
    name        NVARCHAR(128) NOT NULL,
    province    NVARCHAR(32)  NOT NULL,
    level       NVARCHAR(32)  NOT NULL, -- 暂时使用字符串

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_college PRIMARY KEY (id),
    INDEX ix_college_code (code)
);
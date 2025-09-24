CREATE TABLE tbl_web_session
(
    id          BIGINT       NOT NULL,
    session_id  NVARCHAR(50) NOT NULL, -- 44长度就够了的

    create_time DATETIME     NOT NULL,
    update_time DATETIME     NOT NULL,

    CONSTRAINT pk_web_session PRIMARY KEY (id),
    INDEX ix_web_session_session_id (session_id)
);
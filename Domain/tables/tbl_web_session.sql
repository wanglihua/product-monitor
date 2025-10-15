CREATE TABLE tbl_web_session
(
    id           BIGINT       NOT NULL,
    session_id   NVARCHAR(64) NOT NULL, -- 44长度就够了的
    update_count INT          NOT NULL,

    create_time  DATETIME     NOT NULL,
    update_time  DATETIME     NOT NULL,

    CONSTRAINT pk_web_session PRIMARY KEY (id),
);

CREATE UNIQUE INDEX ix_web_session_session_id ON tbl_web_session (session_id);
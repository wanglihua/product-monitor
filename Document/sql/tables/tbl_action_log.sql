CREATE TABLE tbl_action_log
(
    id          BIGINT        NOT NULL,
    type_code   INT           NOT NULL, -- ActionLogType
    user_id     BIGINT        NOT NULL,

    action_desc NVARCHAR(512) NOT NULL DEFAULT '',

    create_time DATETIME      NOT NULL,

    CONSTRAINT pk_action_log PRIMARY KEY (id),
    CONSTRAINT fk_action_log_user_id FOREIGN KEY (user_id) REFERENCES tbl_user (id)
);
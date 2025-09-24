CREATE TABLE tbl_web_session_data
(
    id             BIGINT        NOT NULL,
    web_session_id BIGINT           NOT NULL, -- 44长度就够了的
    type_code      INT           NOT NULL, -- SessionDataType
    data           NVARCHAR(300) NOT NULL, -- 各种数据类型都有可能，长度也不确定，就暂时定个 300长的字符串，应该够用

    create_time    DATETIME      NOT NULL,
    update_time    DATETIME      NOT NULL,

    CONSTRAINT pk_web_session_data PRIMARY KEY (id),
    CONSTRAINT fk_web_session_data_session_id FOREIGN KEY (web_session_id) REFERENCES tbl_web_session (id),
    INDEX ix_web_session_data_update_time (update_time)
);
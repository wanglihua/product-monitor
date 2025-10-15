CREATE TABLE tbl_web_session_data
(
    id             BIGINT        NOT NULL,
    web_session_id BIGINT        NOT NULL,
    session_id     NVARCHAR(64)  NOT NULL,                     --  放上这个 session_id 会带来很多方便
    type_code      INT           NOT NULL,                     -- SessionDataType
    data           NVARCHAR(256) NOT NULL,                     -- 各种数据类型都有可能，长度也不确定，就暂时定个 256 长的字符串，应该够用

    create_time    DATETIME      NOT NULL,
    update_time    DATETIME      NOT NULL,

    CONSTRAINT pk_web_session_data PRIMARY KEY (id),
    -- ON DELETE CASCADE 让 session 过期删除非常方便 
    CONSTRAINT fk_web_session_data_session_id FOREIGN KEY (web_session_id) REFERENCES tbl_web_session (id) ON DELETE CASCADE,
    INDEX ix_web_session_data_web_session_id (web_session_id), --级联删除会用到这个索引
    INDEX ix_web_session_data_session_id (session_id),
    INDEX ix_web_session_data_update_time (update_time)
);
CREATE TABLE tbl_college_change_log
(
    id          BIGINT        NOT NULL,

    college_id  BIGINT        NOT NULL,

    change_desc NVARCHAR(512) NOT NULL DEFAULT '',

    create_time DATETIME      NOT NULL,

    CONSTRAINT pk_college_change_log PRIMARY KEY (id),
    CONSTRAINT fk_college_change_log_college_id FOREIGN KEY (college_id) REFERENCES tbl_college (id),
    INDEX ix_college_change_log_college_id (college_id)
);
CREATE TABLE tbl_test
(
    id          BIGINT        NOT NULL,
    name        NVARCHAR(128) NOT NULL,

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_test PRIMARY KEY (id)
);

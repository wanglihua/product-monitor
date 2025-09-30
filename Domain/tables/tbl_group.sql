CREATE TABLE tbl_group
(
    id          BIGINT        NOT NULL,

    name        NVARCHAR(128) NOT NULL,

    create_time DATETIME      NOT NULL,
    update_time DATETIME      NOT NULL,

    CONSTRAINT pk_group PRIMARY KEY (id)
);
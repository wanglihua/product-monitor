CREATE TABLE tbl_product
(
    id                  BIGINT        NOT NULL,

    name                NVARCHAR(256) NOT NULL,
    version             NVARCHAR(32)  NOT NULL,
    version_update_base NVARCHAR(32)  NOT NULL,
    version_update_desc NVARCHAR(512) NOT NULL,
    version_update_time DATETIME      NOT NULL,

    create_time         DATETIME      NOT NULL,
    update_time         DATETIME      NOT NULL,

    CONSTRAINT pk_product PRIMARY KEY (id)
);
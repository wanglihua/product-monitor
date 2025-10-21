CREATE TABLE tbl_product_url
(
    id             BIGINT        NOT NULL,

    name           NVARCHAR(128) NOT NULL,
    url            NVARCHAR(256) NOT NULL,
    type_code      INT           NOT NULL,

    login_user     NVARCHAR(128) NOT NULL,
    login_password NVARCHAR(128) NOT NULL,

    create_time    DATETIME      NOT NULL,
    update_time    DATETIME      NOT NULL,

    CONSTRAINT pk_product_url PRIMARY KEY (id)
);


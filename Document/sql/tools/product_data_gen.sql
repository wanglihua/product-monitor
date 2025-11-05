INSERT INTO tbl_product(id, name, english_name, version_count, latest_version, version_update_time, create_time, update_time)
SELECT ROW_NUMBER() OVER (ORDER BY name),
       name,
       '',
       0,
       '',
       NULL,
       GETDATE(),
       GETDATE()
FROM tbl_product_name pn;
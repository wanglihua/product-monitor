INSERT INTO tbl_college(id, code, name, province, level, create_time, update_time)
SELECT ROW_NUMBER() OVER (ORDER BY s.identification_code) AS id,
    s.identification_code,
       s.name,
       s.province_type,
       s.educational_level_type,
       GETDATE(),
       GETDATE()
FROM tbl_school s;

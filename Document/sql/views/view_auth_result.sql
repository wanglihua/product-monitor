CREATE OR ALTER VIEW view_auth_result AS

SELECT upr.id          AS id,
       upr.pass_res_id AS pass_res_id,
       upr.user_id     AS user_id
FROM tbl_user_pass_res upr

UNION ALL

SELECT gpr.id + gu.id * 7 AS id,
       gpr.pass_res_id    AS pass_res_id,
       gu.user_id         AS user_id
FROM tbl_group_pass_res gpr
         INNER JOIN tbl_group_user gu ON gpr.group_id = gu.group_id;
GO

CREATE OR ALTER VIEW view_auth_result_detail AS

SELECT upr.id          AS id,
       prc.id          AS pass_res_cate_id,
       prc.name        AS pass_res_cate_name,
       pr.id           AS pass_res_id,
       pr.name         AS pass_res_name,
       u.id            AS user_id,
       u.login_name    AS login_name,
       u.user_name     AS user_name,
       NULL            AS group_id,
       ''              AS group_name,
       upr.create_time AS create_time
FROM tbl_pass_res pr
         INNER JOIN tbl_pass_res_cate prc ON prc.id = pr.cate_id
         INNER JOIN tbl_user_pass_res upr ON upr.pass_res_id = pr.id
         INNER JOIN tbl_user u ON u.id = upr.user_id

UNION ALL

SELECT gpr.id + gu.id * 7 AS id,
       prc.id             AS pass_res_cate_id,
       prc.name           AS pass_res_cate_name,
       pr.id              AS pass_res_id,
       pr.name            AS pass_res_name,
       u.id               AS user_id,
       u.login_name       AS login_name,
       u.user_name        AS user_name,
       g.id               AS group_id,
       g.name             AS group_name,
       gpr.create_time    AS create_time
FROM tbl_pass_res pr
         INNER JOIN tbl_pass_res_cate prc ON prc.id = pr.cate_id
         INNER JOIN tbl_group_pass_res gpr ON gpr.pass_res_id = pr.id
         INNER JOIN tbl_group_user gu ON gpr.group_id = gu.group_id
         INNER JOIN tbl_group g ON gu.group_id = g.id
         INNER JOIN tbl_user u ON u.id = gu.user_id;
GO
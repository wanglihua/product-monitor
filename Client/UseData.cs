using System;

namespace Client
{
    public class UseData
    {
        public string Account { get; set; }

        /// <summary>
        /// 账号类型 管理员，教师，学生
        /// </summary>
        public string AccountType { get; set; }
        
        public DateTime UseTime { get; set; }
    }
}
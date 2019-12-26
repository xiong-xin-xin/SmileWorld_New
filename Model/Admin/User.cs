
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.Admin
{
    /// <summary>
    /// 用户信息表
    /// </summary>
    [Table("base_user")]
    public class User:BaseModel
    {
        public User() { }

        public User(string loginName, string loginPWD)
        {
            LoginName = loginName;
            LoginPWD = loginPWD;
            RealName = loginName;
            Status = 0;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
            LastLoginTime = DateTime.Now;
        }
      
        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string LoginPWD { get; set; }
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        ///最后登录时间 
        /// </summary>
        public DateTime LastLoginTime { get; set; }

        public virtual string[] Roles { get; set; }

        public virtual List<UserRole> UserRoles { get; set; }
    }
}

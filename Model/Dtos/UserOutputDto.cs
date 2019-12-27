using Model.Admin;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Model.Dtos
{
    public class UserOutputDto
    {
        public string Id { get; set; }
        /// <summary>
        /// 登陆名称
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
        /// 城市
        /// </summary>
        public string City { get; set; }
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
        //[JsonIgnore]
        //[IgnoreDataMember]
        public List<UserRole> UserRoles { get; set; }
    }
}

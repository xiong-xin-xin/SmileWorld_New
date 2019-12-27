using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dtos
{
    /// <summary>
    /// 修改密码模型
    /// </summary>
   public class UserPwdInputDto
    {
        /// <summary>
        /// 当前密码
        /// </summary>
        public string currentPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        public string newPassword { get; set; }
    }
}

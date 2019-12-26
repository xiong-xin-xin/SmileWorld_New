using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.Admin
{
    /// <summary>
    /// 用户角色关系表
    /// </summary>
    [Table("base_UserRole")]
    public class UserRole 
    {
        public UserRole() { }

        public UserRole(string uid, string rid)
        {
            Id = Guid.NewGuid().ToString("N").ToUpper();
            UserId = uid;
            RoleId = rid;
            CreatedDate = DateTime.Now;
        }

        /// <summary>
        /// 主键ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 创建人ID
        /// </summary>
        public string CreatedId { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }
        

    }
}

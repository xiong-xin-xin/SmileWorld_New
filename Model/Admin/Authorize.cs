
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.Admin
{
    /// <summary>
    /// 用户权限表
    /// </summary>
    [Table("base_Authorize")]
    public class Authorize
    {
        /// <summary>
        ///  授权功能主键ID
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// 对象分类:1-角色2-用户
        /// </summary>
        public int ObjectType { get; set; }
        /// <summary>
        /// 对象主键.
        /// </summary>
        public string ObjectId { get; set; }
        /// <summary>
        /// 项目ID.例如菜单ID 按钮ID
        /// </summary>
        public string ItemId { get; set; }
        /// <summary>
        /// 项目类型:1-菜单2-按钮
        /// </summary>
        public int ItemType { get; set; }
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


        public virtual Role Role { get; set; }
        public virtual Module Module { get; set; }
        public virtual ModuleButton ModuleButton { get; set; }

    }
}

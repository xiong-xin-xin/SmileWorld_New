using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.Admin
{
    /// <summary>
    /// 角色表
    /// </summary>
    [Table("base_Role")]
    public class Role :BaseModel
    {
        public Role()
        {
        }
        public Role(string name)
        {
            Id = Guid.NewGuid().ToString("N");
            Name = name;
            Description = "";
            OrderSort = 1;
            Enabled = 1;
            CreatedDate = DateTime.Now;
            UpdatedDate = DateTime.Now;
        }
      
        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        ///排序
        /// </summary>
        public int OrderSort { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public int Enabled { get; set; }
       


    }
}

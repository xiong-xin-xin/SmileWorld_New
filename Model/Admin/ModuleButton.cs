using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Model.Admin
{
    /// <summary>
    /// 模块按钮表
    /// </summary>
    [Table("base_ModuleButton")]
    public class ModuleButton : BaseModel
    {
        /// <summary>
        /// 页面Name
        /// </summary>
        public string ModuleName { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public int Enabled { get; set; }
        /// <summary>
        /// 功能主键
        /// </summary>
        public string ModuleId { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// url地址
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string Api { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderSort { get; set; }
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Admin
{
    /// <summary>
    /// 模块功能表
    /// </summary>
    public class Module : BaseModel
    {
      //  public Module()
      //  {
            //this.ChildModule = new List<Module>();
            //this.ModulePermission = new List<ModulePermission>();
            //this.RoleModulePermission = new List<RoleModulePermission>();
       // }

        /// <summary>
        /// 父ID
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 菜单链接地址
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// api
        /// </summary>
        public string Api { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 菜单编号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int OrderSort { get; set; }
        /// <summary>
        /// /描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 是否是右侧菜单
        /// </summary>
        public int IsMenu { get; set; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public int Enabled { get; set; }


        public virtual List<ModuleButton> ModuleButtons { get; set; }
    }
}

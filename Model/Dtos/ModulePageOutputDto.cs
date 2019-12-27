using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dtos
{
    public class ModulePageOutputDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public int IsMenu { get; set; }
        public string Icon { get; set; }

        public string ParentId { get; set; }
  
        public string Name { get; set; }
        /// <summary>
        /// Api
        /// </summary>
        public string Api { get; set; }
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
        /// 是否激活
        /// </summary>
        public int Enabled { get; set; }
        public List<ModulePageOutputDto> Children { get; set; }
    }
}

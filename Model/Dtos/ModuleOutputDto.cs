using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dtos
{
    /// <summary>
    /// 输出参数
    /// </summary>
    public class ModuleOutputDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string CompName { get; set; }
        public string CompPath { get; set; }
        public string Icon { get; set; }
        public List<ModuleOutputDto> Children { get; set; }
    }
}

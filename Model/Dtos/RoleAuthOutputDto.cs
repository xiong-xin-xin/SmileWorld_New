using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dtos
{
    public class RoleAuthOutputDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int IsMenu { get; set; }
        public bool Disabled { get; set; }
        public bool Selected { get; set; }
        public bool Checked { get; set; }
        public List<RoleAuthOutputDto> Children { get; set; }
    }
}

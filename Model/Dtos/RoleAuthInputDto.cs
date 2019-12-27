using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Dtos
{
    /// <summary>
    /// 角色权限模型
    /// </summary>
    public class RoleAuthInputDto
    {
        public AuthList[] AuthLists { get; set; }

        public string RoleId { get; set; }

        public class AuthList
        {
            public int ItemType { get; set; }
            public string ItemId { get; set; }
        }
    }

  
}

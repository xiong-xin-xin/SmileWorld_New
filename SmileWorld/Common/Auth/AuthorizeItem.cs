using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmileWorld.Common.Auth
{
    public class AuthorizeItem
    {
        /// <summary>
        /// 用户或角色或其他凭据名称
        /// </summary>
        public virtual string Role { get; set; }
        /// <summary>
        /// 请求Url
        /// </summary>
        public virtual string Url { get; set; }
    }
}

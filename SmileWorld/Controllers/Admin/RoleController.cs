using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SmileWorld.Controllers.Admin
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleController : BaseController
    {
        private readonly IRoleBLL _roleBLL;
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoleController(IRoleBLL roleBLL)
        {
            _roleBLL = roleBLL;
        }
        /// <summary>
        /// 获取角色列表-key-value
        /// </summary>
        [HttpGet]
        public async Task<object> GetRoleList()
        {
            var data = await _roleBLL.GetRoleList();

            return Success(data);
        }
    }
}
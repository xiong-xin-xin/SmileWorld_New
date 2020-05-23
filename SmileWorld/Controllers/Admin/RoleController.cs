using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Admin;
using Model.Dtos;
using Util;
using static BLL.BaseBLL;

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
        /// 获取角色列表
        /// </summary>
        [HttpGet]
        public async Task<object> GetRolePageList(string pagination, string name)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = await _roleBLL.GetRolePageList(paginationobj, name);
            return Success(data);
        }

        /// <summary>
        /// 修改角色
        /// </summary>-
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult> EditRole([FromForm]Role role)
        {
            return await _roleBLL.EditRoleAsync(role);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        [HttpPost]
        public async Task<AjaxResult> DelUser([FromForm]string ids)
        {
            return await _roleBLL.DelRoleAsync(ids);
        }

        /// <summary>
        /// 获取当前角色的权限
        /// </summary>
        [HttpGet]
        public async Task<object> GetRoleAuthList(string roleId)
        {
            var data = await _roleBLL.GetRoleAuthListAsync(roleId);

            return Success(data);
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        [HttpPost]
        public async Task<AjaxResult> SaveRoleAuth([FromForm]RoleAuthInputDto roleAuthInputDto)
        {
            return await _roleBLL.SaveRoleAuth(roleAuthInputDto);
        }
    }
}
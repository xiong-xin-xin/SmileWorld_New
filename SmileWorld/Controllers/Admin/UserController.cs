using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Util;
using static BLL.BaseBLL;

namespace SmileWorld.Controllers.Admin
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserController : BaseController
    {
        private readonly IUserBLL _userBLL;
        /// <summary>
        /// 构造函数
        /// </summary>
        public UserController(IUserBLL userBLL)
        {
            _userBLL = userBLL;
        }
        /// <summary>
        /// 获取用户分页列表
        /// </summary>
        [HttpGet]
        public async Task<object> GetUserPageList(string pagination, string name)
        {
            Pagination paginationobj = pagination.ToObject<Pagination>();
            var data = await _userBLL.GetUserPageList(paginationobj, name);
            return Success(data);
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult> EditUser([FromForm]User user)
        {
            return  await _userBLL.EditUserAsync(user);

        }
    }
}
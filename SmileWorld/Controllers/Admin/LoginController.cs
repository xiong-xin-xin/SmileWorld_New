using BLL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmileWorld.Common.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmileWorld.Controllers.Admin
{
   
    /// <summary>
    /// 登陆控制器
    /// </summary>
    public class LoginController : BaseController
    {
        private readonly IUserBLL _userBLL;
        private readonly IModuleBLL  _moduleBLL;
        private readonly AuthorizeRequirement _requirement;


        /// <summary>
        /// 构造函数注入
        /// </summary>
        public LoginController(IUserBLL userBLL, IModuleBLL  moduleBLL, AuthorizeRequirement requirement)
        {
            _userBLL = userBLL;
            _moduleBLL = moduleBLL;
            _requirement = requirement;
        }


        /// <summary>
        /// 获取Token的方法
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<object> Token(string username, string password)
        {
            string jwtStr = string.Empty;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(username))
            {
                return new JsonResult(new
                {
                    data = "",
                    code = 1,
                    msg = "用户名或密码不能为空"
                });
            }

            var (user, roles) = await _userBLL.GetUserRoleNameStr(username, password);
            if (user != null)
            {

                //如果是基于用户的授权策略，这里要添加用户;如果是基于角色的授权策略，这里要添加角色
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                     new Claim("roleId", string.Join(',',roles.Select(t=>t.Id))),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Expiration, DateTime.Now.AddSeconds(_requirement.Expiration.TotalSeconds).ToString()) };
                claims.AddRange(roles.Select(s => new Claim(ClaimTypes.Role, s.Name)));

                //用户标识
                var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
                identity.AddClaims(claims);

                //用户页面功能权限查询 
                var dicButton = new Dictionary<string, List<string>>();
                var buttonList = await _moduleBLL.GetUserModuleButtons(user.Id);
                foreach (var item in buttonList)
                {
                    if (item.ModuleName == null) continue;
                    if (dicButton.Keys.Contains(item.ModuleName))
                    {
                        dicButton[item.ModuleName].Add(item.Title);
                    }
                    else
                    {
                        dicButton.Add(item.ModuleName, new List<string>() { item.Title });
                    }
                }
                //var auths = new { MobileSale = new[] { "新增", "修改", "删除", "ด้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้้้็็็็็้้้้้้้้็็็็็้้้" } };

                var token = JwtToken.BuildJwtToken(claims.ToArray(), _requirement);
                return new JsonResult(new
                {
                    data = new { user_id = user.Id, auth_token = token, real_name = user.RealName, auths = dicButton },
                    code = 0,
                    msg = "登陆成功"
                });
            }
            else
            {
                return new JsonResult(new
                {
                    data = "",
                    code = 1,
                    msg = "认证失败"
                });
            }
        }
    }
}
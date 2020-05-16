using DAL;
using Microsoft.AspNetCore.Http;
using Model;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Util;
using Util.Helper;

namespace BLL
{

    public class UserBLL : BaseBLL, IUserBLL
    {
        private readonly IUserDAL _dal;
        private readonly IHttpContextAccessor _accessor;
        public UserBLL(IUserDAL dal, IHttpContextAccessor accessor)
        {
            _dal = dal;
            _accessor = accessor;
        }

        public Task<PageData> GetUserPageList(Pagination pagination, string name)
        {
            var data = _dal.GetUserPageListAsync(pagination, name);
            return data;
        }
        public async Task<AjaxResult> EditUserAsync(User user)
        {
            string userid = _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string uasername = _accessor.HttpContext.User.Identity.Name;

            if (string.IsNullOrWhiteSpace(user.Id))
            {
                user.Id = Guid.NewGuid().ToString("N").ToUpper();
                user.Create(userid, uasername);
                user.LoginPWD = SecurityUtils.EncryptMD5("123456");

                UserRole[] userRoles = user.Roles.Select(t => new UserRole(user.Id, t)
                {
                    CreatedBy = uasername,
                    CreatedId = uasername
                }).ToArray();

                var db = _dal.GetDatabase();
                await db.UseDbTransactionAsync(async (conn, tran) =>
                 {
                     await _dal.DeleteAsync<UserRole>("UserId", user.Id, tran);
                     await _dal.AddAsync(userRoles, tran);
                     await _dal.AddAsync(user, tran);
                 });

                return Success("添加成功");
            }
            else
            {
                user.Update(userid, uasername);
                user.LastLoginTime = DateTime.Now;

                UserRole[] userRoles = user.Roles.Select(t => new UserRole(user.Id, t)
                {
                    CreatedBy = uasername,
                    CreatedId = uasername,

                }).ToArray();

                var db = _dal.GetDatabase();
                await db.UseDbTransactionAsync(async (conn, tran) =>
                {
                    await _dal.DeleteAsync<UserRole>("UserId", user.Id, tran);
                    await _dal.AddAsync(userRoles, tran);
                    await _dal.EditAsync(user, new string[] { "LoginPWD" }, tran);
                });

                return Success("修改成功");
            }

        }

        public async Task<AjaxResult> DelUserAsync(string ids)
        {
            var res = await _dal.DeleteAsync<User>(ids);
            if (res > 0)
            {
                return Success("删除成功");
            }
            else
            {
                return Success("删除失败");
            }
        }

        public async Task<(User, List<Role>)> GetUserRoleNameStr(string loginName, string loginPWD)
        {
            var roles = new List<Role>();

            var user = _dal.GetAllListAsync<User>(new { loginName, LoginPWD = SecurityUtils.EncryptMD5(loginPWD) }).Result.FirstOrDefault();

            if (user != null)
            {
                var userRoles = await _dal.GetAllListAsync<UserRole>(new { UserId = user.Id });
                if (userRoles.Count > 0)
                {
                    roles = await _dal.GetAllListAsync<Role>(new { id = userRoles.Select(ur => ur.RoleId.ToString()) });
                }
            }
            return (user, roles);
        }

        public Task<User> SaveUserInfo(string loginName, string loginPWD)
        {
            throw new NotImplementedException();
        }
    }
}

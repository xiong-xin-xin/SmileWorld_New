using DAL;
using Microsoft.AspNetCore.Http;
using Model;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Util.Helper;

namespace BLL
{
    public class RoleBLL : BaseBLL, IRoleBLL
    {
        private readonly IRoleDAL _dal;
        private readonly IHttpContextAccessor _accessor;
        public RoleBLL(IRoleDAL roleDAL, IHttpContextAccessor accessor)
        {
            _dal = roleDAL;
            _accessor = accessor;
        }

        public async Task<AjaxResult> DelRoleAsync(string ids)
        {
            var res = await _dal.DeleteAsync<Role>(ids);
            if (res > 0)
            {
                return Success("删除成功");
            }
            else
            {
                return Success("删除失败");
            }
        }

        public async Task<AjaxResult> EditRoleAsync(Role role)
        {
            var user = HttpContextCore.GetLoginUser(_accessor);

            if (string.IsNullOrWhiteSpace(role.Id))
            {
                role.Id = Guid.NewGuid().ToString("N").ToUpper();
                role.Create(user.UserId, user.UserName);

                await _dal.AddAsync(role);
                return Success("添加成功");
            }
            else
            {
                role.Update(user.UserId, user.UserName);

                await _dal.EditAsync(role);
                return Success("修改成功");
            }
        }

        public Task<PageData> GetRolePageList(Pagination pagination, string name)
        {
            var data = _dal.GetRolePageListAsync(pagination, name);
            return data;
        }
    }
}

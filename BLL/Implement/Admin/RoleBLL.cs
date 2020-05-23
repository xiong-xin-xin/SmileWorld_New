using DAL;
using Microsoft.AspNetCore.Http;
using Model;
using Model.Admin;
using Model.Dtos;
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
            var res = await _dal.DeleteByIdAsync<Role>(ids);
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

        public async Task<List<RoleAuthOutputDto>> GetRoleAuthListAsync(string roleId)
        {
            var modules = await _dal.GetAllListAsync<Module>();
            var moduleButtons = await _dal.GetAllListAsync<ModuleButton>();

            var rothauth = await _dal.GetAllListAsync<Authorize>(new { ObjectId = roleId, ObjectType = 1 });

            Func<List<Module>, string, List<RoleAuthOutputDto>> res = null;
            res = (List<Module> list, string parentid) =>
            {
                var items = list.FindAll(t => t.ParentId == parentid);

                var b = new List<RoleAuthOutputDto>();
                if (items.Count == 0)
                {
                    var buttons = moduleButtons.FindAll(t => t.ModuleId == parentid);
                    foreach (var item in buttons)
                    {
                        var isHas = rothauth.Exists(t => t.ItemId == item.Id);
                        b.Add(new RoleAuthOutputDto()
                        {
                            Id = item.Id,
                            Title = item.Title,
                            IsMenu = 2,
                            Disabled = item.Enabled == 1 ? false : true,
                            Selected = isHas,
                            Checked = isHas
                        });
                    }
                }
                else
                {
                    items.ForEach(t =>
                    {
                        bool isHas = rothauth.Exists(r => r.ItemId == t.Id);
                        b.Add(new RoleAuthOutputDto()
                        {
                            Id = t.Id,
                            Title = t.Title,
                            IsMenu = t.IsMenu,
                            Disabled = t.Enabled == 1 ? false : true,
                            Selected = isHas,
                            Checked = isHas,
                            Children = res(list, t.Id)
                        });

                    });
                }

                return b;
            };
            return res(modules, "0");
        }

        public Task<PageData> GetRolePageList(Pagination pagination, string name)
        {
            return _dal.GetRolePageListAsync(pagination, name);
        }

        public async Task<AjaxResult> SaveRoleAuth(RoleAuthInputDto roleAuthInputDto)
        {
            var user = HttpContextCore.GetLoginUser(_accessor);

            var db = _dal.GetDatabase();
            await db.UseDbTransactionAsync(async (conn, tran) =>
            {
                await _dal.DeleteAsync<Authorize>(new { ObjectId = roleAuthInputDto.RoleId, ObjectType = 1 }, tran);
                foreach (var auth in roleAuthInputDto.AuthLists)
                {
                    await _dal.AddAsync(new Authorize()
                    {
                        Id = Guid.NewGuid().ToString("N"),
                        ObjectId = roleAuthInputDto.RoleId,
                        ObjectType = 1,
                        ItemId = auth.ItemId,
                        ItemType = auth.ItemType,
                        CreatedId = user.UserId,
                        CreatedBy = user.UserName,
                        CreatedDate = DateTime.Now
                    }, tran);
                }
            });

            return Success("保存成功");
        }
    }
}

using DAL;
using Microsoft.AspNetCore.Http;
using Model;
using Model.Admin;
using Model.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Helper;

namespace BLL
{
    public class ModuleBLL : BaseBLL, IModuleBLL
    {
        private readonly IModuleDAL _moduleDAL;
        private readonly IHttpContextAccessor _accessor;
        public ModuleBLL(IModuleDAL moduleDAL, IHttpContextAccessor accessor)
        {
            _moduleDAL = moduleDAL;
            _accessor = accessor;
        }

        public async Task<AjaxResult> DelModuleAsync(string ids)
        {
            var res = await _moduleDAL.DeleteAsync<Module>(ids);
            var r = await _moduleDAL.DeleteAsync<ModuleButton>(ids);
            if (res > 0 || r > 0)
            {
                return Success("删除成功");
            }
            else
            {
                return Success("删除失败");
            }
        }

        public async Task<AjaxResult> EditModuleAsync(Module module)
        {
            var user = HttpContextCore.GetLoginUser(_accessor);

            if (string.IsNullOrWhiteSpace(module.Id))
            {
                module.Id = Guid.NewGuid().ToString("N").ToUpper();
                module.Create(user.UserId, user.UserName);

                if (module.IsMenu == 1)//菜单
                {
                    await _moduleDAL.AddAsync(module);
                }
                if (module.IsMenu == 2)//按钮
                {
                    var button = AutoMapper.Mapper.Map<ModuleButton>(module);
                    await _moduleDAL.AddAsync(button);
                }
                return Success("添加成功");
            }
            else
            {
                module.Update(user.UserId, user.UserName);

                if (module.IsMenu == 1)//菜单
                {
                    await _moduleDAL.EditAsync(module);
                }
                if (module.IsMenu == 2)//按钮
                {
                    var button = AutoMapper.Mapper.Map<ModuleButton>(module);
                    await _moduleDAL.EditAsync(button);
                }
                return Success("修改成功");
            }
        }

        public async Task<PageData> GetModulePageListAsync(Pagination pagination, string title)
        {
            var modules = await _moduleDAL.GetModulePageListAsync(pagination, title);
            var moduleButtons = await _moduleDAL.GetAllListAsync<ModuleButton>();

            Func<List<Module>, string, List<ModulePageOutputDto>> res = null;
            //递归0.0
            res = (List<Module> list, string parentid) =>
            {
                var items = list.FindAll(t => t.ParentId == parentid).OrderBy(t => t.OrderSort).ToList();

                var b = new List<ModulePageOutputDto>();
                //按钮
                if (items.Count == 0)
                {
                    var buttons = moduleButtons.FindAll(t => t.ModuleId == parentid);
                    foreach (var item in buttons)
                    {
                        b.Add(new ModulePageOutputDto()
                        {
                            Id = item.Id,
                            Title = item.Title,
                            Icon = item.Icon,
                            Path = item.Path,
                            Name = item.ModuleName,
                            Api = item.Api,
                            IsMenu = 2,
                            OrderSort = item.OrderSort,
                            Description = item.Description,
                            Enabled = item.Enabled,
                            ParentId = parentid
                        });
                    }
                }
                else //菜单
                {
                    items.ForEach(t =>
                    {
                        b.Add(new ModulePageOutputDto()
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Icon = t.Icon,
                            Name = t.Title,
                            Path = t.Path,
                            Api = t.Api,
                            IsMenu = t.IsMenu,
                            OrderSort = t.OrderSort,
                            Description = t.Description,
                            Enabled = t.Enabled,
                            ParentId = parentid,
                            Children = res(list, t.Id)
                        });
                    });
                }
                return b;
            };
            var r = new PageData()
            {
                data = res(modules.ToList(), "0"),
                total = pagination.total
            };
            return r;
        }

        public Task<List<Module>> GetUserModule(string userId)
        {
            return _moduleDAL.GetUserModuleAsync(userId);
        }
        public Task<List<ModuleButton>> GetUserModuleButtons(string userId)
        {
            return _moduleDAL.GetUserModuleButtonsAsync(userId);
        }
    }
}

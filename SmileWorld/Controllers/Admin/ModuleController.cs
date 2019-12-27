using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Admin;
using Model.Dtos;

namespace SmileWorld.Controllers.Admin
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class ModuleController : BaseController
    {
        private readonly IModuleBLL _moduleBLL;
        public ModuleController(IModuleBLL moduleBLL)
        {
            _moduleBLL = moduleBLL;

        }

        /// <summary>
        /// 获取用户菜单列表
        /// </summary>
        [HttpGet("getusermodulelist")]
        public async Task<object> GetUserModuleList()
        {
            var userid = this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value;
            var data = await _moduleBLL.GetUserModule(userid);
            Func<List<Module>, string, List<ModuleOutputDto>> res = null;
            //递归0.0
            res = (List<Module> list, string parentid) =>
            {
                var items = list.FindAll(t => t.ParentId == parentid).OrderBy(t => t.OrderSort).ToList();

                var b = new List<ModuleOutputDto>();

                items.ForEach(t =>
                {
                    b.Add(new ModuleOutputDto()
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Icon = t.Icon,
                        Name = t.Title,
                        CompName = t.Name,
                        CompPath = t.Path,
                        Path = t.Path,
                        Children = res(list, t.Id)
                    });
                });
                return b;
            };
            return Success(res(data, "0"));
        }
    }
}
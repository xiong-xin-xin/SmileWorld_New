using Model;
using Model.Admin;
using Model.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static BLL.BaseBLL;

namespace BLL
{
    public interface IModuleBLL
    {
        Task<List<Module>> GetUserModule(string userId);
        Task<List<ModuleButton>> GetUserModuleButtons(string userId);

        Task<PageData> GetModulePageListAsync(Pagination pagination, string title);

        Task<AjaxResult> EditModuleAsync(Module module);

        Task<AjaxResult> DelModuleAsync(string ids);

    }
}

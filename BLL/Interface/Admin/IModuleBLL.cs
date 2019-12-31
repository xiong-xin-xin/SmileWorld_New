using Model.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IModuleBLL
    {
        Task<List<Module>> GetUserModule(string userId);
        Task<List<ModuleButton>> GetUserModuleButtons(string userId);

    }
}

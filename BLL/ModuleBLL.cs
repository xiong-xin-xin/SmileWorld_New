using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Model.Admin;

namespace BLL
{
    public class ModuleBLL : IModuleBLL
    {
        private readonly IModuleDAL _moduleDAL;
        public ModuleBLL(IModuleDAL moduleDAL)
        {
            _moduleDAL = moduleDAL;

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

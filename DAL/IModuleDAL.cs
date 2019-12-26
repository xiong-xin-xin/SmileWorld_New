using Model.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public interface IModuleDAL: IBaseDao
    {
        Task<List<ModuleButton>> GetUserModuleButtonsAsync(string userId);

    }
}

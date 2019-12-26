using Model.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IRoleModuleAuthorizeBLL
    {
        Task<List<Authorize>> GetRoleModule();
        //Task<List<Authorize>> TestModelWithChildren();
        //Task<List<Authorize>> GetRoleModuleByRoleAndUserId(string role, string userId);


        //Task<List<Authorize>> WithChildrenModel();
    }
}

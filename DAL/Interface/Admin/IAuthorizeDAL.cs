using Model.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IAuthorizeDAL: IBaseDao
    {
        Task<List<Authorize>> GetRoleModuleByRoleAndUserId(string role, string userId);

        Task<List<Authorize>> AuthorizeWithChildrenModel();
    }
}

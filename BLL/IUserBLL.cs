using Model;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IUserBLL
    {
        Task<ResponeInfo> GetUserPageList(Pagination pagination, string name);
        Task<User> SaveUserInfo(string loginName, string loginPWD);
        Task<(User, List<Role>)> GetUserRoleNameStr(string loginName, string loginPWD);
    }
}

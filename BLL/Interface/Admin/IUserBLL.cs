using Model;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static BLL.BaseBLL;

namespace BLL
{
    public interface IUserBLL
    {
        Task<PageData> GetUserPageList(Pagination pagination, string name);
        Task<AjaxResult> EditUserAsync(User user);
        Task<AjaxResult> DelUserAsync(string ids);
        Task<User> SaveUserInfo(string loginName, string loginPWD);
        Task<(User, List<Role>)> GetUserRoleNameStr(string loginName, string loginPWD);
    }
}

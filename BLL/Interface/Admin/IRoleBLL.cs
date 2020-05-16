using Model;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static BLL.BaseBLL;

namespace BLL
{
    public interface IRoleBLL
    {
        Task<PageData> GetRolePageList(Pagination pagination, string name);

        Task<AjaxResult> EditRoleAsync(Role role);
        Task<AjaxResult> DelRoleAsync(string ids);
    }
}

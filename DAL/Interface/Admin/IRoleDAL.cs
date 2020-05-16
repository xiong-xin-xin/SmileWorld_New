using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IRoleDAL:IBaseDao
    {
        Task<PageData> GetRolePageListAsync(Pagination pagination, string name);
    }
}

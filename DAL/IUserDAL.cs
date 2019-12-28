using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUserDAL : IBaseDao
    {
        Task<ResponeInfo> GetUserPageListAsync(Pagination pagination, string name);
        Task GetUserAsync();
    }
}

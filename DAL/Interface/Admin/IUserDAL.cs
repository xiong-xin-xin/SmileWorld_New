using Model;
using Model.Admin;
using Model.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IUserDAL : IBaseDao
    {
        Task<PageData> GetUserPageListAsync(Pagination pagination, string name);
        Task GetUserAsync();
    }
}

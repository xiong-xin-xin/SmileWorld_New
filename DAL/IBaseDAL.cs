using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IBaseDao
    {
        Task<IEnumerable<T>> GetPageListAsync<T>(Pagination pagination) where T : class, new();

        Task<List<T>> GetAllListAsync<T>(object where = null) where T : class, new();
    }
}

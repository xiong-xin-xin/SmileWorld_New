using DB;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IBaseDao
    {
        IDatabase GetDatabase();
        Task<IEnumerable<T>> GetPageListAsync<T>(Pagination pagination) where T : class, new();
        Task<List<T>> GetAllListAsync<T>(object where = null) where T : class, new();
        Task<T> QueryFirstAsync<T>(string vue, string key = "Id") where T : class, new();
        Task<int> DeleteAsync<T>(string vue, IDbTransaction tran = null) where T : class, new();
        Task<int> DeleteAsync<T>(string vue, string key, IDbTransaction tran = null) where T : class, new();
        Task<int> AddAsync<T>(T[] model, IDbTransaction tran = null) where T : class, new();
        Task<int> AddAsync<T>(T model, IDbTransaction tran = null) where T : class, new();
        Task<int> EditAsync<T>(T model, string[] ignoreColumns = default(string[]), IDbTransaction tran = null) where T : class, new();
        Task<int> EditAsync<T>(T model, string[] updateColumns, string[] ignoreColumns, IDbTransaction tran = null) where T : class, new();
    }
}

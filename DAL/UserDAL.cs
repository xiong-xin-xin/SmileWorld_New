using Dapper;
using DB;
using Model;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace DAL
{
    public class UserDAL : BaseDao, IUserDAL
    {
        public UserDAL(IDatabase database) : base(database) { }
        public async Task GetUserAsync()
        {
            string sql = "select * from Base_User";
            var a = await _database.UseDbConnectionAsync(t => t.QueryAsync<dynamic>(sql));
        }

        public async Task<ResponeInfo> GetUserPageListAsync(Pagination pagination, string name)
        {
            pagination.sql = "select * from base_User";
            if (!string.IsNullOrWhiteSpace(name))
            {
                pagination.sql += " where LoginName=@name ";
                pagination.where = new { name };
            }
            ResponePageData data = new ResponePageData();
            data.data = await GetPageListAsync<User>(pagination);
            data.total = pagination.records;

            return new ResponeInfo(data);
        }
    }
}

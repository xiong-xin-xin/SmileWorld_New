using Dapper;
using DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace DAL
{
    public class UserDAL: BaseDao, IUserDAL
    {
        public UserDAL(IDatabase database) : base(database) { }
        public async Task GetUserAsync()
        {
            string sql = "select * from LR_Base_User";
            var a =await _database.UseDbConnectionAsync(t=>t.QueryAsync<dynamic>(sql));
        }

    }
}

using Dapper;
using DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace DAL
{
    public class UserInfoDAL: IUserInfoDAL
    {
        private IDatabase  _database;
        public UserInfoDAL(IDatabase database)
        {
            _database = database;
        }
        public async Task GetUserInfoAsync()
        {
            string sql = "select * from LR_Base_User";
            var a =await _database.UseDbConnectionAsync(t=>t.QueryAsync<dynamic>(sql));
        }

    }
}

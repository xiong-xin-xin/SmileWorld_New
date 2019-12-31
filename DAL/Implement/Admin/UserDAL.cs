using Dapper;
using DB;
using Model;
using Model.Admin;
using Model.Dtos;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
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

        public async Task<PageData> GetUserPageListAsync(Pagination pagination, string name)
        {
            pagination.sql = "select * from base_User";
            if (!string.IsNullOrWhiteSpace(name))
            {
                pagination.sql += " where LoginName=@name ";
                pagination.where = new { name };
            }
            var user = await GetPageListAsync<UserOutputDto>(pagination);

            var userRoles = await GetAllListAsync<UserRole>(new { UserId = user.AsList().Select(t => t.Id) });
            foreach (var item in user)
            {
                item.UserRoles = userRoles.Where(t => t.UserId == item.Id).ToList();
            }

            PageData data = new PageData
            {
                data = user,
                total = pagination.records
            };

            return data;
        }
    }
}

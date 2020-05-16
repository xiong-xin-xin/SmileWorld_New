using DB;
using Model;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class RoleDAL : BaseDao, IRoleDAL
    {
        public RoleDAL(IDatabase database) : base(database) { }
        public async Task<PageData> GetRolePageListAsync(Pagination pagination, string name)
        {
            pagination.sql = "select * from base_role";
            if (!string.IsNullOrWhiteSpace(name))
            {
                pagination.sql += " where name=@name ";
                pagination.where = new { name };
            }
            pagination.sidx = "ordersort";
            pagination.sord = "asc";
            var roleList = await GetPageListAsync<Role>(pagination);

            var data = new PageData
            {
                data = roleList,
                total = pagination.records
            };
            return data;
        }
    }
}

using DB;
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
        public async Task<object> GetRoleListAsync()
        {
            var data = await GetAllListAsync<Role>();
            return data.ToList().Select(f => new
            {
                value = f.Id,
                label = f.Name
            });
        }
    }
}

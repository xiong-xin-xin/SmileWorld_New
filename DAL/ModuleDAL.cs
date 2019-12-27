using Dapper;
using DB;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ModuleDAL : BaseDao, IModuleDAL
    {
        public ModuleDAL(IDatabase database) : base(database) { }
        public async Task<List<Module>> GetUserModuleAsync(string userId)
        {
            string sql = @"select distinct a.* from base_module a                            join base_authorize b on a.id =b.itemid  
                            join base_role c on b.objectid=c.id
                            join base_userrole d on d.roleid=c.id
                            join base_user e on e.id=d.userid
                            where b.itemtype=1 and b.objecttype=1 and e.id=@userId order by a.orderSort";
            var data = await _database.UseDbConnectionAsync(t => t.QueryAsync<Module>(sql, new { userId }));
            return data.ToList();
        }

        public async Task<List<ModuleButton>> GetUserModuleButtonsAsync(string userId)
        {
            string sql = @"select b.* from base_authorize a,base_modulebutton b where a.itemid = b.id  and a.itemtype = 2
                                and exists(select* from base_userrole ur where ur.roleid= a.objectid and ur.userId= @userId)";
            var data = await _database.UseDbConnectionAsync(t => t.QueryAsync<ModuleButton>(sql, new { userId }));
            return data.ToList();
        }
    }
}

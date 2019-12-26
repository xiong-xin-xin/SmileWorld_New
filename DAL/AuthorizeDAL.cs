using DB;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AuthorizeDAL : BaseDao, IAuthorizeDAL
    {
        public AuthorizeDAL(IDatabase database) : base(database) { }

        public Task<List<Authorize>> AuthorizeWithChildrenModel()
        {

            throw new NotImplementedException();
        }

        public Task<List<Authorize>> GetRoleModuleByRoleAndUserId(string role, string userId)
        {
            string sql = $"select * from base_Authorize a where a.ObjectId=@userId  ";

            throw new NotImplementedException();
        }
    }
}

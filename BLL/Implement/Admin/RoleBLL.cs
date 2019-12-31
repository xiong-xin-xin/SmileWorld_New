using DAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RoleBLL : IRoleBLL
    {
        private readonly IRoleDAL _roleDAL;
        public RoleBLL(IRoleDAL roleDAL)
        {
            _roleDAL = roleDAL;
        }

        public Task<object> GetRoleList()
        {
            return _roleDAL.GetRoleListAsync();
        }
    }
}

using DAL;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Util;
using Util.Helper;

namespace BLL
{

    public class UserBLL :IUserBLL
    {
        private readonly IUserDAL _dal;
        public UserBLL(IUserDAL dal)
        {
            _dal = dal;
        }
        public async Task<(User, List<Role>)> GetUserRoleNameStr(string loginName, string loginPWD)
        {
            var roles = new List<Role>();

            var user = _dal.GetAllListAsync<User>(new { loginName, LoginPWD = SecurityUtils.EncryptMD5(loginPWD) }).Result.FirstOrDefault();

            if (user != null)
            {
                var userRoles = await _dal.GetAllListAsync<UserRole>(new { UserId = user.Id });
                if (userRoles.Count > 0)
                {
                    roles = await _dal.GetAllListAsync<Role>(new { id = userRoles.Select(ur => ur.RoleId.ToString()).ToArray() });
                }
            }
            return (user, roles);
        }

        public Task<User> SaveUserInfo(string loginName, string loginPWD)
        {
            throw new NotImplementedException();
        }
    }
}

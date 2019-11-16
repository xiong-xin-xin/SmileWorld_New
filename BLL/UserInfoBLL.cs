using DAL;
using System;
using System.Threading.Tasks;

namespace BLL
{
    public class UserInfoBLL: IUserInfoBLL
    {
        private IUserInfoDAL _dal;
        public UserInfoBLL(IUserInfoDAL dal)
        {
            _dal = dal;
        }
        public async Task GetUserInfoAsync()
        {
           await _dal.GetUserInfoAsync();
        }
    }
}

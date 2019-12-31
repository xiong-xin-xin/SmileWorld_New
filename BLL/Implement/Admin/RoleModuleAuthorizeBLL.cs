using DAL;
using Model.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RoleModuleAuthorizeBLL : IRoleModuleAuthorizeBLL
    {
        private readonly IModuleDAL _moduleDAL;

        public RoleModuleAuthorizeBLL(IModuleDAL moduleDAL)
        {
            _moduleDAL = moduleDAL;
        }

        /// <summary>
        /// 获取全部 角色接口(按钮)关系数据
        /// </summary>
        /// <returns></returns>
       // [Caching(AbsoluteExpiration = 10)]
        public async Task<List<Authorize>> GetRoleModule()
        {
            var roleModulePermissions = await _moduleDAL.GetAllListAsync<Authorize>(new { ItemType = 2 });
            if (roleModulePermissions.Count > 0)
            {
                var roles =await _moduleDAL.GetAllListAsync<Role>();
                var modulebuttons =await _moduleDAL.GetAllListAsync<ModuleButton>();

                foreach (var item in roleModulePermissions)
                {
                    item.Role = roles.Where(t => t.Id == item.ObjectId).FirstOrDefault();
                    item.ModuleButton = modulebuttons.Where(t => t.Id == item.ItemId).FirstOrDefault();
                }
            }
            return await Task.FromResult(roleModulePermissions);
        }

        //public async Task<List<Authorize>> GetRoleModuleByRoleAndUserId(string role, string userId)
        //{
        //    string[] roles = role.Split(',');
        //    return await this.Query(t => t.ObjectId == userId || roles.Contains(t.ObjectId));
        //}

        //public async Task<List<Authorize>> TestModelWithChildren()
        //{
        //    return await this.WithChildrenModel();
        //}


        //public async Task<List<Authorize>> WithChildrenModel()
        //{
        //    var list = await Task.Run(() => Db.Queryable<Authorize>()
        //            .Mapper(it => it.Role, it => it.ObjectId)
        //            .Mapper(it => it.Module, it => it.ObjectId).ToList());

        //    return list;
        //}


    }
}

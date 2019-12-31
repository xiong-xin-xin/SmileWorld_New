using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public interface IRoleBLL
    {
        Task<object> GetRoleList();

    }
}

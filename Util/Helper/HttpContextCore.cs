using Microsoft.AspNetCore.Http;
using Model;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Util.Helper
{
    public class HttpContextCore
    {
        public static LoginUser GetLoginUser(IHttpContextAccessor httpContextAccessor)
        {
            var user = new LoginUser();
            user.UserId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            user.UserName = httpContextAccessor.HttpContext.User.Identity.Name;
            return user;
        }
    }
}

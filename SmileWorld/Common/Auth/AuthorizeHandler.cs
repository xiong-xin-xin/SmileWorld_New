using BLL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmileWorld.Common.Auth
{
    /// <summary>
    /// 权限授权处理器
    /// </summary>
    public class AuthorizeHandler : AuthorizationHandler<AuthorizeRequirement>
    {
        /// <summary>
        /// 验证方案提供对象
        /// </summary>
        public IAuthenticationSchemeProvider Schemes { get; set; }

        /// <summary>
        /// services 层注入
        /// </summary>
        private readonly IRoleModuleAuthorizeBLL  _roleModuleAuthorizeBLL;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        public AuthorizeHandler(IAuthenticationSchemeProvider schemes, IRoleModuleAuthorizeBLL roleModuleAuthorizeBLL)
        {
            Schemes = schemes;
            _roleModuleAuthorizeBLL = roleModuleAuthorizeBLL;
        }

        /// <summary>
        /// 重载异步处理程序
        /// </summary>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizeRequirement requirement)
        {
            // 将最新的角色和接口列表更新
            var data = await _roleModuleAuthorizeBLL.GetRoleModule();
            var list = (from item in data
                        orderby item.Id
                        select new AuthorizeItem
                        {
                            Url = item.ModuleButton?.Api,
                            Role = item.Role?.Name,
                        }).ToList();
            requirement.AuthorizeItems = list;

            //从AuthorizationHandlerContext转成HttpContext，以便取出表求信息
            var httpContext = (context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext).HttpContext;
            //请求Url
            var questUrl = httpContext.Request.Path.Value.ToLower();
            //判断请求是否停止
            var handlers = httpContext.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
            foreach (var scheme in await Schemes.GetRequestHandlerSchemesAsync())
            {
                var handler = await handlers.GetHandlerAsync(httpContext, scheme.Name) as IAuthenticationRequestHandler;
                if (handler != null && await handler.HandleRequestAsync())
                {
                    context.Fail();
                    return;
                }
            }
            //判断请求是否拥有凭据，即有没有登录
            var defaultAuthenticate = await Schemes.GetDefaultAuthenticateSchemeAsync();
            if (defaultAuthenticate != null)
            {
                var result = await httpContext.AuthenticateAsync(defaultAuthenticate.Name);
                //result?.Principal不为空即登录成功
                if (result?.Principal != null)
                {

                    httpContext.User = result.Principal;
                    //权限中是否存在请求的url
                    if (requirement.AuthorizeItems.Exists(g => g.Url?.ToLower() == questUrl))
                    {
                        // 获取当前用户的角色信息
                        var currentUserRoles = httpContext.User.Claims.Where(item => item.Type == requirement.ClaimType)
                                                                       .Select(i => i.Value)
                                                                       .ToList();
                        //验证权限
                        if (!requirement.AuthorizeItems.Exists(t => currentUserRoles.Contains(t.Role) && t.Url?.ToLower() == questUrl))
                        {
                            context.Fail();
                            return;
                        }
                    }
                    else
                    {
                        // context.Fail();
                        // return;
                    }
                    //判断过期时间
                    if ((httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) != null && DateTime.Parse(httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Expiration)?.Value) >= DateTime.Now)
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                        return;
                    }
                    return;
                }
            }
            //判断没有登录时，是否访问登录的url
            if (!questUrl.Equals(requirement.LoginPath.ToLower(), StringComparison.Ordinal))
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
        }
    }
}

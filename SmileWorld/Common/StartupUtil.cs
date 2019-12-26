using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SmileWorld.Common.Auth;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmileWorld.Common
{
    public static class StartupUtil
    {
        public static void RegisterAssemblyTypes(this IServiceCollection serviceCollection, Assembly assembly, string endWith)
        {
            var types = assembly.GetTypes().Where(x => x.IsClass && !x.IsAbstract);
            if (!string.IsNullOrEmpty(endWith))
            {
                types = types.Where(x => x.Name.EndsWith(endWith));
            }
            var interfaces = assembly.GetTypes().Where(w => w.IsInterface);
            if (!string.IsNullOrEmpty(endWith))
            {
                interfaces = interfaces.Where(x => x.Name.EndsWith(endWith));
            }
            var interfaceArray = interfaces as Type[] ?? interfaces.ToArray();
            foreach (var type in types)
            {
                var interfaceType = interfaceArray.FirstOrDefault(w => w.IsAssignableFrom(type));
                if (interfaceType != null)
                {
                    serviceCollection.AddScoped(interfaceType, type);
                }
            }
        }


        public static void AddSwagger(this IServiceCollection services)
        {
            var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(c =>
            {
                new string[] { "admin", "api" }.ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new Info
                    {
                        Version = version,
                        Title = $"SmileWorld 接口文档",
                        Description = "SmileWorld.Api HTTP API ",
                        TermsOfService = "None",
                        Contact = new Contact { Name = "xxx", Email = "xiong_xin_xin@qq.com", Url = "http://smileworld.tech/" }
                    });
                });


                // 按相对路径排序，作者：Alby
                c.OrderActionsBy(o => o.RelativePath);

                var xmlPath = Path.Combine(basePath, nameof(SmileWorld) + ".xml");//这个就是刚刚配置的xml文件名
                c.IncludeXmlComments(xmlPath, true);//默认的第二个参数是false，这个是controller的注释，记得修改

                var xmlModelPath = Path.Combine(basePath, nameof(Model) + ".xml");//这个就是Model层的xml文件名
                c.IncludeXmlComments(xmlModelPath);
                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();

            });
        }

        public static void AddJwt(this IServiceCollection services, IConfiguration configuration)
        {
            //读取配置文件
            var audienceConfig = configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],//发行人
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],//订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 如果要数据库动态绑定，这里先留个空，后边处理器里动态赋值
            var permission = new List<AuthorizeItem>();

            // 角色与接口的权限要求参数
            var permissionRequirement = new AuthorizeRequirement(
                "/api/denied",// 拒绝授权的跳转地址（目前无用）
                permission,
                ClaimTypes.Role,//基于角色的授权
                audienceConfig["Issuer"],//发行人
                audienceConfig["Audience"],//听众
                signingCredentials,//签名凭据
                expiration: TimeSpan.FromHours(6)//接口的过期时间
                );


            //【授权】
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Permission", policy => policy.Requirements.Add(permissionRequirement));
            })
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = tokenValidationParameters;
                o.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // 如果过期，则把<是否过期>添加到，返回头信息中
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/hub")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddSingleton<IAuthorizationHandler, AuthorizeHandler>();
            services.AddSingleton(permissionRequirement);

        }

    }
}

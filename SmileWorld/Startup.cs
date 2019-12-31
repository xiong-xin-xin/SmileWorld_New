
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using DAL;
using DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmileWorld.Common;
using SmileWorld.Common.Filter;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.System.Text.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Util;

namespace SmileWorld
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 此方法由运行时调用。使用此方法将服务添加到容器。
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(o=>{
                o.Filters.Add(typeof(GlobalExceptionsFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton(Configuration);
            services.AddScoped<IDbConnection>(x => new SqlConnection(Configuration["ConnectionStrings:BaseDb"]));
            services.AddScoped<IDatabase, Database>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddSingleton(redisConfiguration);
            services.AddSingleton<IRedisCacheClient, RedisCacheClient>();
            services.AddSingleton<IRedisCacheConnectionPoolManager, RedisCacheConnectionPoolManager>();
            services.AddSingleton<IRedisDefaultCacheClient, RedisDefaultCacheClient>();
            services.AddSingleton<ISerializer, SystemTextJsonSerializer>();

            services.RegisterAssemblyTypes(Assembly.Load("DAL"), "DAL");
            services.RegisterAssemblyTypes(Assembly.Load("BLL"), "BLL");
            //根据属性注入来配置全局拦截器
            services.ConfigureDynamicProxy(config =>
            {
                ServiceProvider provider = services.BuildServiceProvider();
                //拦截代理所有BLL结尾的类
                config.Interceptors.AddTyped<ToolRedisCacheAOP>(new object[] { provider.GetService<IRedisCacheClient>() }, Predicates.ForService("*BLL"));
            });
            services.AddSwagger();

            services.AddCors(c =>
            {
                //全开放式跨域
                c.AddPolicy("AllRequests", policy =>
                {
                    policy
                    .AllowAnyOrigin()//允许任何源
                    .AllowAnyMethod()//允许任何方式
                    .AllowAnyHeader()//允许任何头
                    .AllowCredentials();//允许cookie
                });

                c.AddPolicy("LimitRequests", policy =>
                {
                    policy
                    .WithOrigins("http://localhost:8080", "http://172.20.63.173:8080")//支持多个域名端口，注意端口号后不要带/斜杆
                    .AllowAnyHeader()//Ensures that the policy allows any header.
                    .AllowCredentials()
                    .AllowAnyMethod();
                });
            });
            services.AddJwt(Configuration);

            return services.BuildDynamicProxyServiceProvider();
        }

        // 此方法由运行时调用。使用此方法配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                new string[] { "admin", "api"  }.ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
                });
            });

            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseCors("AllRequests");
            }
            else
            {
                app.UseCors("LimitRequests");
            }

            //设置默认页
            app.UseDefaultFiles();
            // 使用静态文件
            app.UseStaticFiles();
            // 返回错误码
            app.UseStatusCodePages();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
  
}

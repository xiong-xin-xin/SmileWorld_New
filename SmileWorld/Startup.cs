using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton(Configuration);
            services.AddScoped<IDbConnection>(x => new SqlConnection(Configuration["ConnectionStrings:BaseDb"]));
            services.AddScoped<IDatabase, Database>();

            var redisConfiguration = Configuration.GetSection("Redis").Get<RedisConfiguration>();
            services.AddSingleton(redisConfiguration);
            services.AddSingleton<IRedisCacheClient, RedisCacheClient>();
            services.AddSingleton<IRedisCacheConnectionPoolManager, RedisCacheConnectionPoolManager>();
            services.AddSingleton<IRedisDefaultCacheClient, RedisDefaultCacheClient>();
            services.AddSingleton<ISerializer, SystemTextJsonSerializer>();

            services.RegisterAssemblyTypes(Assembly.Load("BLL"), "BLL");
            services.RegisterAssemblyTypes(Assembly.Load("DAL"), "DAL");
            //根据属性注入来配置全局拦截器
            services.ConfigureDynamicProxy(config =>
            {
                ServiceProvider provider = services.BuildServiceProvider();
                //拦截代理所有BLL结尾的类
                config.Interceptors.AddTyped<ToolRedisCacheAOP>(new object[] { provider.GetService<IRedisCacheClient>() }, Predicates.ForService("*BLL"));
            });
            return services.BuildDynamicProxyServiceProvider();
        }

        // 此方法由运行时调用。使用此方法配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
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

    }
}

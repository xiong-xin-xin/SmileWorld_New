using DB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SmileWorld
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddScoped<IDbConnection>(x => new SqlConnection(Configuration["ConnectionString:BaseDb"]));
            services.AddScoped<IDatabase, Database>();

            services.RegisterAssemblyTypes(Assembly.Load("BLL"), "BLL");
            services.RegisterAssemblyTypes(Assembly.Load("DAL"), "DAL");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
        public static void RegisterAssemblyTypes(this IServiceCollection serviceCollection, Assembly assembly,string endWith)
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

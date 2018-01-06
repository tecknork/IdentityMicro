using IdentityMicro.Business.UserManager;
using IdentityMicro.Data.DBContext;
using IdentityMicro.Entities.IdentityModels;
using IdentityMicro.Entities.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityMicro.Business.IocManager
{
    public static class StartupConfigure 
    {
        

        public static void ConfigureIOC(IServiceCollection services, IConfiguration configuration)
        {
            

            services.AddDbContext<GADDBContext>(options =>
                  options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            
            var idsrvBuilder = services.AddIdentityServer()
              .AddDeveloperSigningCredential()
              //.AddInMemoryIdentityResources(Config.GetIdentityResources())
              //.AddInMemoryApiResources(Config.GetApiResources())
              //.AddInMemoryClients(Config.GetClients())
              //.AddTestUsers(Config.GetUsers())
              .AddConfigurationStore(options =>
              {
                  options.ConfigureDbContext = builder =>
                         builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                             sql => sql.MigrationsAssembly("IdentityMicro.Data"));

                 
              })

              .AddOperationalStore(options =>
              {
                  options.ConfigureDbContext = builder =>
                        builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                            sql => sql.MigrationsAssembly("IdentityMicro.Data"));

              });

            services.AddScoped<IUserRepository, UserRepository>();

           idsrvBuilder.AddResourceOwnerValidator<ResourceOwnerPasswordValidator>();
           idsrvBuilder.AddProfileService<ProfileService>();

           
            services.AddScoped<UserStore>();


        }

        public static void ConfigureApp(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseIdentityServer();
        }
    }
}

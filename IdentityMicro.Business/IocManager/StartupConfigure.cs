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

            services.AddIdentity<User, IdentityRole>(options =>
            {
                // example of setting options
                options.Tokens.ChangePhoneNumberTokenProvider = "Phone";

                // password settings chosen due to NIST SP 800-63
                options.Password.RequiredLength = 8; // personally i'd prefer to see 10+
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<GADDBContext>()
            .AddDefaultTokenProviders();

      
            
            var idsrvBuilder = services.AddIdentityServer()
              .AddDeveloperSigningCredential()
                 //.AddInMemoryIdentityResources(Config.GetIdentityResources())
                 //.AddInMemoryApiResources(Config.GetApiResources())
                 //.AddInMemoryClients(Config.GetClients())
                 //.AddTestUsers(Config.GetUsers())
               .AddAspNetIdentity<User>()
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

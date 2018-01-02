using IdentityMicro.Data.DBContext;
using IdentityMicro.Entities.IdentityModels;
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

            services.AddIdentity<GADUser, IdentityRole>((options =>
                       options.Password.RequireNonAlphanumeric = false))
                      .AddEntityFrameworkStores<GADDBContext>();
        }
    }
}

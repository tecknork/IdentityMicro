using IdentityMicro.Data.DBContext;
using IdentityMicro.Entities.Interfaces;
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

            //services.AddIdentity<ConfArchUser, IdentityRole>((options =>
            //            options.Password.RequireNonAlphanumeric = false))
            //    .AddEntityFrameworkStores<ConfArchDbContext>();
        }
    }
}

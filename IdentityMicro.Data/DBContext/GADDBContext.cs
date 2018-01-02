using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityMicro.Data.DBContext
{
   public class GADDBContext : IdentityDbContext<Entities.IdentityModels.GADUser>
    {
        public GADDBContext(DbContextOptions<GADDBContext> options)
            : base(options)
        {

        }

       


    }
}

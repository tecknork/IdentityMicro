using System;
using System.Collections.Generic;
using System.Text;
using IdentityMicro.Entities.IdentityModels;
using IdentityMicro.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityMicro.Data.DBContext
{
   public class GADDBContext : DbContext
   {
        public GADDBContext(DbContextOptions<GADDBContext> options)
            : base(options)
        {

        }

        public DbSet<GADUser> Users { get; set; }


    }
}

﻿using System;
using System.Collections.Generic;
using System.Text;
using IdentityMicro.Entities.IdentityModels;

using Microsoft.EntityFrameworkCore;

namespace IdentityMicro.Data.DBContext
{
   public class GADDBContext : DbContext
    {
        public GADDBContext(DbContextOptions<GADDBContext> options)
            : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserExternalProvider> UserExternalProviders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(i => i.UserName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(i => i.SubjectId).IsUnique();
            modelBuilder.Entity<UserExternalProvider>().HasIndex(i => i.ProviderName);
            modelBuilder.Entity<UserExternalProvider>().HasIndex(i => i.ProviderSubjectId);
            modelBuilder.Entity<UserExternalProvider>().HasIndex(i => new { i.ProviderName, i.ProviderSubjectId }).IsUnique();
        }



    }
}

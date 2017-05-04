﻿using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jahshaka.Core.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        public ApplicationDbContext()
        {

        }

        public virtual DbSet<Asset> Assets { get; set;}
        public new virtual DbSet<ApplicationUser> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Asset>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.HasOne(a => a.User).WithMany( a => a.Assets).HasForeignKey(e => e.UserId);

            });

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable<ApplicationUser>("Users");
            });

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
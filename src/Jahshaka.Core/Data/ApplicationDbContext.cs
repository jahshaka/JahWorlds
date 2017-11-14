using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Jahshaka.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, Guid, IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options) { }

        public ApplicationDbContext()
        {

        }
        public virtual DbSet<Application> Applications { get; set; }
        public virtual DbSet<ApplicationVersion> ApplicationVersions { get; set; }
        public virtual DbSet<Asset> Assets { get; set;}
        public new virtual DbSet<Role> Roles { get; set; }
        public new virtual DbSet<ApplicationUser> Users { get; set; }
        public virtual DbSet<World> Worlds { get; set;}
        public virtual DbSet<WorldVersion> WorldVersions { get; set;}
        public virtual DbSet<WorldVersionAsset> WorldVersionAssets { get; set;}
        public virtual DbSet<XmlKey> XmlKeys { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Application>(entity =>
            {
                entity.ToTable<Application>("OpenIddictApplications");

                entity.HasMany(e => e.Versions).WithOne(e => e.Application).HasForeignKey(e => e.ApplicationId);
            });

            builder.Entity<Authorization>(entity =>
            {
                entity.ToTable<Authorization>("OpenIddictAuthorizations");
            });

            builder.Entity<Scope>(entity =>
            {
                entity.ToTable<Scope>("OpenIddictScopes");
            });

            builder.Entity<Token>(entity =>
            {
                entity.ToTable<Token>("OpenIddictTokens");
            });

            builder.Entity<Asset>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.HasOne(a => a.User).WithMany( a => a.Assets).HasForeignKey(e => e.UserId);

            });

            builder.Entity<World>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.HasOne(a => a.User).WithMany( a => a.Worlds).HasForeignKey(e => e.UserId);

            });
            
            builder.Entity<WorldVersion>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.HasOne(a => a.World).WithMany( a => a.WorldVersions).HasForeignKey(e => e.WorldId);
            });
            
            builder.Entity<WorldVersionAsset>(entity =>
            {
                entity.HasKey(e => new { e.AssetId, e.WorldVersionId });
                
                entity.HasOne(a => a.Asset).WithMany( a => a.WorldVersionAssets).HasForeignKey(e => e.AssetId);
                entity.HasOne(a => a.WorldVersion).WithMany( a => a.WorldVersionAssets).HasForeignKey(e => e.WorldVersionId);
                entity.HasOne(a => a.World).WithMany( a => a.WorldVersionAssets).HasForeignKey(e => e.WorldId);
            });
            
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable<ApplicationUser>("Users");
            });
            
            builder.Entity<XmlKey>(entity =>
            {
                entity.HasKey(e => e.Name);
            });

            builder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
            });

            builder.Entity<UserRole>(entity =>
            {
                entity.ToTable<UserRole>("UserRoles");
            });

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}

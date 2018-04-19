using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Jahshaka.Core.Data;

namespace Jahshaka.AuthServer.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseNpgsql("User ID=postgres;Password=JahWap20UpAndOut;Server=159.203.93.225;Port=5432;Database=jahshaka;Pooling=true;", b => b.MigrationsAssembly("Jahshaka.AuthServer"));

            return new ApplicationDbContext(builder.Options);
        }
    }
}
using DataLayer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DatabaseLayer
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated(); // Ensure that the database schema is created
            ShellScriptExecutor executor = new ShellScriptExecutor();
        }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Announcements>? Announcements { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        }

    }
}

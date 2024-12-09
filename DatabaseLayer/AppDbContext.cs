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
        }
        public DbSet<Product>? Products { get; set; }
        public DbSet<Announcements>? Announcements { get; set; }

    }
}

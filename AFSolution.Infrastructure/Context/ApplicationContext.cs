using AFSolution.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFSolution.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.Entity<Role>().HasData(
                new Role("Admin") { Id = new Guid("7fbe872e-571a-4f9c-b26a-ed0fee611a67"), Name = "Admin" },
                new Role("Admin") { Id = new Guid("560f7545-f89e-401b-b663-3b5d54951d48"), Name = "Default" }
            );
        }
    }
}

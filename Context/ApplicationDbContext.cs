using JWT_AuthAndRefrest.Models.Entities;
using JWT_AuthAndRefrest.Models.Entities.Seeding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace JWT_AuthAndRefrest.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            SeedingInicial.Seed(modelBuilder);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            
        }

        public DbSet<User> Users => Set<User>(); 
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>(); 
    }
}

using DreamScape.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamScape.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;" +
                "port=3306;" +
                "user=root;" +
                "password=;" +
                "database=csd_DreamScape",
                ServerVersion.Parse("10.4.17-mariadb"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>() // Store Role as a string in db to avoid complexity
                .Property(user => user.Role)
                .HasConversion<string>();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "123@123.com",
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123"),
                    Role = Role.Player
                },
                new User
                {
                    Id = 2,
                    Email = "345@345.com",
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword("345"),
                    Role = Role.Player
                },
                new User
                {
                    Id = 3,
                    Email = "admin@example.com",
                    Password = BCrypt.Net.BCrypt.EnhancedHashPassword("123"),
                    Role = Role.Admin
                }
            );
        }
    }
}

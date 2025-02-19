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
        public DbSet<Item> Items { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }

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
            modelBuilder.Entity<User>() // Store Role and item rarity as a string in db to avoid complexity
                .Property(user => user.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Item>()
                .Property(item => item.Rarity)
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

            modelBuilder.Entity<Item>().HasData(
                new Item
                {
                    Id = 1,
                    Name = "Sword",
                    Description = "It's an Awesome Sword!",
                    Type = "Melee Weapon",
                    Rarity = Rarity.Common
                },
                new Item
                {
                    Id = 2,
                    Name = "Shield",
                    Description = "It's a Shield!",
                    Type = "Defense",
                    Rarity = Rarity.Uncommon
                },
                new Item
                {
                    Id = 3,
                    Name = "Bow",
                    Description = "It's a Bow!",
                    Type = "Ranged Weapon",
                    Rarity = Rarity.Rare,
                    AdditionalEffects = "Requires arrows to use!"
                }
            );

            modelBuilder.Entity<PlayerItem>().HasData(
                new PlayerItem
                {
                    Id = 1,
                    UserId = 1,
                    ItemId = 1,
                },
                new PlayerItem
                {
                    Id = 2,
                    UserId = 1,
                    ItemId = 2,
                },
                new PlayerItem
                {
                    Id = 3,
                    UserId = 2,
                    ItemId = 3,
                },
                new PlayerItem
                {
                    Id = 4,
                    UserId = 2,
                    ItemId = 1,
                },
                new PlayerItem
                {
                    Id = 5,
                    UserId = 2,
                    ItemId = 2,
                },
                new PlayerItem
                {
                    Id = 6,
                    UserId = 1,
                    ItemId = 3,
                }
            );
        }
    }
}

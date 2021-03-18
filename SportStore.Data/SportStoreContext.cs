using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportStore.Models.Entities;
using System;

namespace SportStore.Data
{
    public class SportStoreContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }

        public SportStoreContext(DbContextOptions<SportStoreContext> options) : base(options) 
        {
            Database.EnsureCreated(); 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            var hasher = new PasswordHasher<User>();

            // Seeding roles.
            builder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int>[]
                {
                    new IdentityRole<int>
                    {
                        Id = 1,
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR"
                    },
                    new IdentityRole<int>
                    {
                        Id = 2,
                        Name = "User",
                        NormalizedName = "USER"
                    }
                });

            // Seeding administrator.
            builder.Entity<User>().HasData(
                new User[]
                {
                    new User
                    {
                        Id = 1,
                        UserName = "admin",
                        NormalizedUserName = "ADMIN",
                        PasswordHash = hasher.HashPassword(null, "Pa$$w0rd"),
                        SecurityStamp = Guid.NewGuid().ToString()
                    }
                });

            // Seeding relations between admin and roles.
            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int>[]
                {
                    new IdentityUserRole<int>
                    {
                        RoleId = 1,
                        UserId = 1
                    },
                    new IdentityUserRole<int>
                    {
                        RoleId = 2,
                        UserId = 1
                    }
                });
        }
    }
}

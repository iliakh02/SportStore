using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SportStore.Models.Entities;

namespace SportStore.Data
{
    public class SportStoreContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }

        public SportStoreContext(DbContextOptions<SportStoreContext> options) : base(options) { Database.EnsureCreated(); }
    }
}

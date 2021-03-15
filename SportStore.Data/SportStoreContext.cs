using Microsoft.EntityFrameworkCore;
using SportStore.Models.Entities;

namespace SportStore.Data
{
    public class SportStoreContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }

        public SportStoreContext(DbContextOptions options) : base(options) { Database.EnsureCreated(); }
    }
}

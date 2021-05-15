using SportStore.Models.Entities;

namespace SportStore.Data.Abstract
{
    public interface ICartRepository : IEntityBaseRepository<CartItem> { }
    public interface ICategoryRepository : IEntityBaseRepository<Category> { }
    public interface IOrderRepository : IEntityBaseRepository<Order> { }
    public interface IProductOrderRepository : IEntityBaseRepository<ProductOrder> { }
    public interface IProductRepository : IEntityBaseRepository<Product> { }
}

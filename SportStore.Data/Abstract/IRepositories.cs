using SportStore.Models.Entities;

namespace SportStore.Data.Abstract
{
    public interface ICartRepository : IEntityBaseRepository<Cart> { }
    public interface ICategoryRepository : IEntityBaseRepository<Category> { }
    public interface IOrderRepository : IEntityBaseRepository<Order> { }
    public interface IProductOrderRepository : IEntityBaseRepository<ProductOrder> { }
    public interface IProductRepository : IEntityBaseRepository<Product> { }
    public interface IRoleRepository : IEntityBaseRepository<Role> { }
    public interface IUserRepository : IEntityBaseRepository<User> { }
    public interface IPaymentRepository : IEntityBaseRepository<Payment> { }
}

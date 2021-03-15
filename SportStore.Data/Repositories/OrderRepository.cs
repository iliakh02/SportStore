using SportStore.Models.Entities;
using SportStore.Data.Abstract;

namespace SportStore.Data.Repositories
{
    public class OrderRepository : EntityBaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(SportStoreContext context) : base(context) { }
    }
}

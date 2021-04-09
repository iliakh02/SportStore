using SportStore.Data.Abstract;
using SportStore.Models.Entities;

namespace SportStore.Data.Repositories
{
    public class OrderRepository : EntityBaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(SportStoreContext context) : base(context) { }
    }
}

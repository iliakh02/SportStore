using SportStore.Models.Entities;
using static SportStore.Data.Abstract.IRepositories;

namespace SportStore.Data.Repositories
{
    public class OrderRepository : EntityBaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(SportStoreContext context) : base(context) { }
    }
}

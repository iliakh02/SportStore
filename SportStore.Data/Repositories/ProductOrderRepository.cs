using SportStore.Models.Entities;
using static SportStore.Data.Abstract.IRepositories;

namespace SportStore.Data.Repositories
{
    public class ProductOrderRepository : EntityBaseRepository<ProductOrder>, IProductOrderRepository
    {
        public ProductOrderRepository(SportStoreContext context) : base(context) { }
    }
}

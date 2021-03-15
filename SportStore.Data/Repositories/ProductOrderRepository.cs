using SportStore.Models.Entities;
using SportStore.Data.Abstract;

namespace SportStore.Data.Repositories
{
    public class ProductOrderRepository : EntityBaseRepository<ProductOrder>, IProductOrderRepository
    {
        public ProductOrderRepository(SportStoreContext context) : base(context) { }
    }
}

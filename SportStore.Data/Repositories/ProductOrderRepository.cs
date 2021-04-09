using SportStore.Data.Abstract;
using SportStore.Models.Entities;

namespace SportStore.Data.Repositories
{
    public class ProductOrderRepository : EntityBaseRepository<ProductOrder>, IProductOrderRepository
    {
        public ProductOrderRepository(SportStoreContext context) : base(context) { }
    }
}

using SportStore.Data.Abstract;
using SportStore.Models.Entities;

namespace SportStore.Data.Repositories
{
    public class ProductRepository : EntityBaseRepository<Product>, IProductRepository
    {
        public ProductRepository(SportStoreContext context) : base(context) { }
    }
}

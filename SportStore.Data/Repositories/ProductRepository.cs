using SportStore.Models.Entities;
using static SportStore.Data.Abstract.IRepositories;

namespace SportStore.Data.Repositories
{
    public class ProductRepository : EntityBaseRepository<Product>, IProductRepository
    {
        public ProductRepository(SportStoreContext context) : base(context) { }
    }
}

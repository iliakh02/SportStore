using SportStore.Models.Entities;
using static SportStore.Data.Abstract.IRepositories;

namespace SportStore.Data.Repositories
{
    public class ProductCartRepository : EntityBaseRepository<ProductCart>, IProductCartRepository
    {
        public ProductCartRepository(SportStoreContext context) : base(context) { }
    }
}

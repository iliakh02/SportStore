using SportStore.Models.Entities;
using SportStore.Data.Abstract;

namespace SportStore.Data.Repositories
{
    public class ProductCartRepository : EntityBaseRepository<ProductCart>, IProductCartRepository
    {
        public ProductCartRepository(SportStoreContext context) : base(context) { }
    }
}

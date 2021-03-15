using SportStore.Models.Entities;
using static SportStore.Data.Abstract.IRepositories;

namespace SportStore.Data.Repositories
{
    public class CartRepository : EntityBaseRepository<Cart>, ICartRepository
    {
        public CartRepository(SportStoreContext context) : base(context) { }
    }
}

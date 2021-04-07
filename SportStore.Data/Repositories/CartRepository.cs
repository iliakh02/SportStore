using SportStore.Models.Entities;
using SportStore.Data.Abstract;

namespace SportStore.Data.Repositories
{
    public class CartRepository : EntityBaseRepository<CartItem>, ICartRepository
    {
        public CartRepository(SportStoreContext context) : base(context) { }
    }
}

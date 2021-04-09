using SportStore.Data.Abstract;
using SportStore.Models.Entities;

namespace SportStore.Data.Repositories
{
    public class CartRepository : EntityBaseRepository<CartItem>, ICartRepository
    {
        public CartRepository(SportStoreContext context) : base(context) { }
    }
}

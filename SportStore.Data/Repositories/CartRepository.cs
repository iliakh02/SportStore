using Microsoft.EntityFrameworkCore;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SportStore.Data.Repositories
{
    public class CartRepository : EntityBaseRepository<CartItem>, ICartRepository
    {
        public CartRepository(SportStoreContext context) : base(context) { }
        public override List<CartItem> GetAll()
        {
            var carts = _context.Carts.Include(n => n.Product).AsNoTracking().ToList();
            return carts;
        }
    }
}

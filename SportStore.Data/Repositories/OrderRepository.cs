using Microsoft.EntityFrameworkCore;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SportStore.Data.Repositories
{
    public class OrderRepository : EntityBaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(SportStoreContext context) : base(context) { }

        public override List<Order> GetAll()
        {
            return _context.Orders.Include(n => n.ProductOrders).ThenInclude(n => n.Product).ToList();
        }
    }
}

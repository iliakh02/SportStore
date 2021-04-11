using Microsoft.EntityFrameworkCore;
using SportStore.Data.Abstract;
using SportStore.Models.Entities;
using System.Collections.Generic;
using SportStore.Data;
using System;
using System.Linq;

namespace SportStore.Data.Repositories
{
    public class ProductRepository : EntityBaseRepository<Product>, IProductRepository
    {
        public ProductRepository(SportStoreContext context) : base(context) { }

        public override List<Product> GetAll()
        {
            return _context.Products.Include(n => n.Category).ToList();
        }
        public override Product GetById(int id)
        {
            return _context.Products.Include(n => n.Category).FirstOrDefault(x => x.Id == id);
        }
    }
}

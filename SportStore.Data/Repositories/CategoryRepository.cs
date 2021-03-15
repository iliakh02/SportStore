using SportStore.Models.Entities;
using SportStore.Data.Abstract;

namespace SportStore.Data.Repositories
{
    public class CategoryRepository : EntityBaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(SportStoreContext context) : base(context) { }
    }
}

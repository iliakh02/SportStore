using SportStore.Data.Abstract;
using SportStore.Models.Entities;

namespace SportStore.Data.Repositories
{
    public class CategoryRepository : EntityBaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(SportStoreContext context) : base(context) { }
    }
}

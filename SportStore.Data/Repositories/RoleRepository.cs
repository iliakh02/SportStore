using SportStore.Models.Entities;
using SportStore.Data.Abstract;

namespace SportStore.Data.Repositories
{
    public class RoleRepository : EntityBaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(SportStoreContext context) : base(context) { }
    }
}

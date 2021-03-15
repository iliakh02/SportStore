using SportStore.Models.Entities;
using static SportStore.Data.Abstract.IRepositories;

namespace SportStore.Data.Repositories
{
    public class UserRepository : EntityBaseRepository<User>, IUserRepository
    {
        public UserRepository(SportStoreContext context) : base(context) { }
    }
}

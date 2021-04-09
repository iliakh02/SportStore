using SportStore.Data.Abstract;
using SportStore.Models.Entities;

namespace SportStore.Data.Repositories
{
    public class PaymentRepository : EntityBaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(SportStoreContext context) : base(context) { }
    }
}

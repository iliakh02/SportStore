using SportStore.Models.Entities;
using SportStore.Data.Abstract;

namespace SportStore.Data.Repositories
{
    public class PaymentRepository : EntityBaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(SportStoreContext context) : base(context) { }
    }
}

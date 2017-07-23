using Microsoft.EntityFrameworkCore;
using PaymentGateway.DataModel.DomainModel;

namespace PaymentGateway.DataModel
{
    public class PaymentRepository : Repository<Payment>
    {
        private DbSet<Payment> _paymentEntity;
        public PaymentRepository(DbContext context) : base(context)
        {
            _paymentEntity = context.Set<Payment>();
        }
    }
}

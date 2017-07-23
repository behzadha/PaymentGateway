using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.DataModel.DomainModel;

namespace PaymentGateway.DataModel
{
    public interface IPaymentVerificationRepository : IRepository<PaymentVerification>
    {

    }
    public class PaymentVerificationRepository : Repository<PaymentVerification>, IPaymentVerificationRepository
    {
        public PaymentVerificationRepository(DbContext context) : base(context)
        {
        }


    }
}

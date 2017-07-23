using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.DataModel
{
    public static class DbInitializer
    {
        public static void Initialize(PaymentGateway.DataModel.DomainModel.PaymentGatewayContext context)
        {
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}

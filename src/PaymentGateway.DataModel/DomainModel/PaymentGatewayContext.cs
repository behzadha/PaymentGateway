using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.DataModel.DomainModel
{
    public class PaymentGatewayContext : DbContext
    {
        public PaymentGatewayContext(DbContextOptions<PaymentGatewayContext> options ):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new PaymentMap(modelBuilder.Entity<Payment>());
            new PaymentVerificationMap(modelBuilder.Entity<PaymentVerification>());
            new BankGatewayMap(modelBuilder.Entity<BankGateway>());
            new GatewayCreateTokenStatusMap(modelBuilder.Entity<GatewayResponseStatus>());
            new GatewayTokenMap(modelBuilder.Entity<GatewayToken>());
        }

    }
}

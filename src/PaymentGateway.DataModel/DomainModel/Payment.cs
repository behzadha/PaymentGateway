using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PaymentGateway.DataModel.DomainModel
{
    public class Payment
    {
        public int Id { get; set; }
        public int BankGatewayId { get; set; }
        public string PaymentToken { get; set; }
        public long InvoiceNumber { get; set; }
        public long RequestId { get; set; }
        public decimal Amount { get; set; }
        public DateTime EntryDate { get; set; }
        public PaymentVerification TransactionVerification { get; set; }
        public GatewayToken GatewayToken { get; set; }
        public BankGateway BankGateway { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public string OriginalStatusId { get; set; }
    }

    public class PaymentMap
    {
        public PaymentMap(EntityTypeBuilder<Payment> entityBuilder)
        {
            
        }
    }

}

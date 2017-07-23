using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.DataModel.DomainModel
{
    public class PaymentVerification
    {
        [Key]
        public int Id { get; set; }
        public int PaymentId { get; set; }
        public string OriginalStatusId { get; set; }
        public string ReferenceNumber { get; set; }
        public bool IsVerified { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime EntryDate { get; set; }
        public Payment Payment { get; set; }
    }
    public class PaymentVerificationMap
    {
        public PaymentVerificationMap(EntityTypeBuilder<PaymentVerification> entityBuilder)
        {
            entityBuilder.HasOne(k => k.Payment);
        }
    }
}

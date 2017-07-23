using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.DataModel.DomainModel
{
    public class GatewayToken
    {
        public int Id { get; set; }
        public int PaymentId { get; set; }
        [MaxLength(70)]
        public string Token { get; set; }
        [MaxLength(5)]
        public string OriginalStatusId { get; set; }
        public Payment Payment { get; set; }
        public DateTime EntryDate { get; set; }
    }
    public class GatewayTokenMap
    {
        public GatewayTokenMap(EntityTypeBuilder<GatewayToken> entityBuilder)
        {
            entityBuilder.HasOne(k => k.Payment);
        }
    }
}

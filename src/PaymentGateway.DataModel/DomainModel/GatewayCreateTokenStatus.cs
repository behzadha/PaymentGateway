using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.DataModel.DomainModel
{
    public class GatewayResponseStatus
    {
        [Key]
        public int Id { get; set; }
        public int BankGatewayId { get; set; }
        [MaxLength(5)]
        public string OriginalStatusId { get; set; }
        [MaxLength(200)]
        public string Message { get; set; }
        public BankGateway BankGateway { get; set; }
     //   public ICollection<GatewayToken> GatewayTokens { get; set; }
      //  public ICollection<PaymentVerification> PaymentVerifications { get; set; }
    }
    public class GatewayCreateTokenStatusMap
    {
        public GatewayCreateTokenStatusMap(EntityTypeBuilder<GatewayResponseStatus> entityBuilder)
        {

            //entityBuilder.HasMany(k => k.GatewayTokens);
            //entityBuilder.HasMany(k => k.PaymentVerifications);
            entityBuilder.HasOne(k => k.BankGateway);
        }
    }
}
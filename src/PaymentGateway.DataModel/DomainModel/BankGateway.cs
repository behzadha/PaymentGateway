using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PaymentGateway.DataModel.DomainModel
{
    public class BankGateway
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(25)]
        public string GatewayName { get; set; }
        [MaxLength(300)]
        public string Url { get; set; }
        [MaxLength(20)]
        public string Terminal { get; set; }
        [MaxLength(30)]
        public string Username { get; set; }
        [MaxLength(30)]
        public string Password { get; set; }
        public ICollection<Payment> Payments { get; set; }
        public bool IsActive { get; set; }
    }
    public class BankGatewayMap
    {
        public BankGatewayMap(EntityTypeBuilder<BankGateway> entityBuilder)
        {
            entityBuilder.HasMany(k => k.Payments);
        }
    }
}

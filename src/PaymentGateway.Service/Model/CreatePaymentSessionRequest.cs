using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Model
{
    public class PaymentSessionRequest
    {
        public string PaymentToken { get; set; }
        public long InvoiceNumber { get; set; }
        public decimal TotalPrice { get; set; } 
        public int GatewayId { get; set; }
        public string Description { get; set; }
    }

    public class PaymentSessionResponse : PaymentSessionRequest
    {
        public int PaymentId { get; set; }
        public DateTime EntryDate { get; set; }
        public string Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public Verification Verification { get; set; }
        public GatewayToken GatewayToken { get; set; }
    }

    public class GatewayToken
    {
        public DateTime EntryDate { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }
    }
    public class Verification
    {
        public DateTime VerificationDate { get; set; }
        public string Status { get; set; }
        public string ReferenceNumber { get; set; }
    }
}

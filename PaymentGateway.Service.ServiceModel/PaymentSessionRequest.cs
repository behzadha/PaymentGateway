using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Service.ServiceModel
{
    public class PaymentSessionRequest
    {
        public string PaymentToken { get; set; }
        public long InvoiceNumber { get; set; }
        public decimal TotalPrice { get; set; } 
        public int GatewayId { get; set; }
        public string Description { get; set; }
    }


}

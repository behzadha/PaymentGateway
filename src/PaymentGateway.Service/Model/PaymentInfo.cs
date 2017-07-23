using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Model
{
    public class PaymentInfo
    {
        public string Token { get; set; }
        public DateTime EntryDate { get; set; }
        public Decimal Amount { get; set; }
        public long InvoiceNumber { get; set; }
        public long RequestId { get; set; }
    }
}

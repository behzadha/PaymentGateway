using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.Service.Model
{
    public class PaymentVerification
    {
        public int PaymentId { get; set; }
        public string StatusId { get; set; }
        public string ReferenceNumber { get; set; }
        public bool IsVerified { get; set; }
    }
}

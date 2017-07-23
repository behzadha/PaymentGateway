using System;
using PaymentGateway.BankGatewayProvider;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway
{
    public class SamanBankGatewayConfiguration:IBankGatewayConfiguration
    {
        public SamanBankGatewayConfiguration()
        {

        }

        public string UserId { get; set; }
        public string Password { get; set; }
        public string TerminalId { get; set; }
        public string GatewayUrl { get; set; }
        public string CallbackUrl { get; set; }
        public bool IsDefault { get; set; }
        public int GatewayId { get; set; }
    }
}

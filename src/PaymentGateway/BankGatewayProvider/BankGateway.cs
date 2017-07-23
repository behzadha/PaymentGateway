using System.Threading.Tasks;
using PaymentGateway.Model;
using System.Collections.Generic;

namespace PaymentGateway.BankGatewayProvider
{
    public abstract class BankGateway : IBankGatewayProvider
    {
       // private string _gatewayCode;
        private IBankGatewayConfiguration bankGatewayConfigurations;

        public BankGateway(IBankGatewayConfiguration bankGatewayConfiguration)
        {
            this.bankGatewayConfigurations = bankGatewayConfiguration;
        //    _gatewayCode = gatewayCode;
        }


        public IBankGatewayConfiguration Configuration => bankGatewayConfigurations;

      //  public string GatewayCode => _gatewayCode;

        //public IBankGatewayConfiguration Configuration => throw new NotImplementedException();

        public abstract  Task<CreateTokenResponse> CreateToken(CreateTokenRequest createTokenRequest);
        public abstract  Task<VerifyPaymentResponse> VerifyPayment(VerifyPaymentRequest verifyRequest);
        public abstract string PreparePOSTForm(string url, Dictionary<string, string> data);
    }
}

using System.Threading.Tasks;
using PaymentGateway.Model;
using System.Collections.Generic;

namespace PaymentGateway.BankGatewayProvider
{
    public interface IBankGatewayProvider
    {
        IBankGatewayConfiguration Configuration { get; }
    //   string GatewayCode { get; }
        Task<CreateTokenResponse> CreateToken(CreateTokenRequest createTokenRequest );
        Task<VerifyPaymentResponse> VerifyPayment(VerifyPaymentRequest verifyRequest);
        string PreparePOSTForm(string url, Dictionary<string, string> data);
    }
}

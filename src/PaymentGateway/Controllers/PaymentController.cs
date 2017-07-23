using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.BankGatewayProvider;
using Microsoft.AspNetCore.Http;
using PaymentGateway.Model;
using Microsoft.Extensions.Options;
using PaymentGateway.Service.ServiceInterface;
namespace PaymentGateway.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IEnumerable<IBankGatewayProvider> _bankGatewayProvider;
        private readonly ConfigurationSettings _settings;
        public PaymentController(IPaymentService paymentService, IEnumerable<IBankGatewayProvider> bankGatewayProvider, IOptions<ConfigurationSettings> settings)
        {
            _paymentService = paymentService;
            _bankGatewayProvider = bankGatewayProvider;
            _settings = settings.Value;
        }

        [HttpGet]
        public async Task<IEnumerable<Service.ServiceModel.BankGateway>> BankGateway()
        {
            return await _paymentService.BankGateways(isActive: true);
        }

        [HttpGet]
        public async Task<Service.ServiceModel.BankGateway> BankGateway(int id)
        {
            var gws = await _paymentService.BankGateways(null);
            return gws.FirstOrDefault(k => k.BankGatewayId == id);
        }


        [HttpPost]
        public async Task<Service.ServiceModel.PaymentSessionResponse> PaymentSession([FromBody]Service.ServiceModel.PaymentSessionRequest createPaymentSessionRequest)
        {
            var token = await _paymentService.CreatePaymentSession(createPaymentSessionRequest);
            return token;
        }
        [HttpGet]
        public async Task<Service.ServiceModel.PaymentSessionResponse> PaymentSession(string token)
        {
            var payment = await _paymentService.GetPaymentSession(token);
            return payment;
        }


        [HttpGet]
        [Produces("text/html")]
        public async Task<string> RedirectToGateway(string token)
        {
            try
            {
                var paymentInfo = await _paymentService.GetPaymentSession(token);
                var backGateway = _bankGatewayProvider.First(k => k.Configuration.GatewayId == paymentInfo.GatewayId);
                if (paymentInfo == null)
                {
                    throw new Exception("Invalid token in redirectToGateway method, the token is not created.");
                }
                if ((DateTime.Now - paymentInfo.EntryDate).Minutes > _settings.SessionTimeout.Minutes)
                {
                    throw new Exception("The token has been expired. Consider calling CreatePaymentSession method once more.");
                }
                var a = await _bankGatewayProvider.First().CreateToken(new Model.CreateTokenRequest()
                {
                    TotalAmount = (long)paymentInfo.TotalPrice,
                    CallbackUrl = backGateway.Configuration.CallbackUrl,
                    InvoiceNumber = paymentInfo.InvoiceNumber.ToString()
                });

                await _paymentService.AddGatewayToken(paymentInfo.PaymentId, a.OriginalErrorId.ToString(), a.Token);

                if (a.IsSuccessful)
                {
                    Dictionary<string, string> datacollection = new Dictionary<string, string>();
                    datacollection.Add("Token", a.Token);
                    datacollection.Add("RedirectURL", "http://localhost:5000/payment/callback?token=" + paymentInfo.PaymentToken);

                    var postData = _bankGatewayProvider.First().PreparePOSTForm("https://sep.shaparak.ir/payment.aspx", datacollection);//


                    return postData;
                    //var response = Response;
                    //response.Headers.Add("location", "https://sep.shaparak.ir/Payment.aspx");
                    // return response;
                }
                else
                {
                    var response = Response;
                    response.Headers.Add("location", "/Error");
                    return "";
                    //  return response;
                }
            }
            catch (Exception ex)
            {
                return "";
                //throw;
            }
        }

        [HttpPost]
        public async Task Callback(string token)
        {
            try
            {
                var str = Request.Form;

                if (string.IsNullOrEmpty(token))
                {
                    throw new Exception("token is null or empty!");
                }
                var paymentInfo = await _paymentService.GetPaymentSession(token);


                if (paymentInfo == null)
                {
                    throw new Exception("Invalid token in redirectToGateway method, the token is not created.");
                }
                if (paymentInfo.Verification != null)
                {
                    throw new Exception($"Token : {token} - has been verified already.");
                }
                if ((DateTime.Now - paymentInfo.EntryDate).Minutes > 20)
                {
                    throw new Exception($"Token : {token} - has been expired.");
                }

                var referenceNumber = string.Empty;
                var saleReferenceId = string.Empty;
                var merchantId = string.Empty;
                var state = string.Empty;
                var stateCode = string.Empty;
                //saman
                if (paymentInfo.GatewayId == 1)
                {
                    referenceNumber = GetStringParam("RefNum");
                    saleReferenceId = GetStringParam("ResNum");
                    merchantId = GetStringParam("MID");
                    state = GetStringParam("state");
                    stateCode = GetStringParam("stateCode");
                }

                await _paymentService.UpdatePaymentStatus(token, stateCode);


                if (!string.IsNullOrEmpty(referenceNumber))
                {
                    var gatewayCallbackResult = await _bankGatewayProvider.First(k => k.Configuration.GatewayId == paymentInfo.GatewayId).VerifyPayment(new Model.VerifyPaymentRequest
                    {
                        ReferenceNumber = referenceNumber,
                        SaleReferenceId = saleReferenceId,
                        MerchantId = merchantId
                    });


                    await _paymentService.PaymentVerification(new Service.ServiceModel.PaymentVerification { IsVerified = gatewayCallbackResult.IsVerified, PaymentId = paymentInfo.PaymentId, StatusId = gatewayCallbackResult.OriginalStatus });

                    if (gatewayCallbackResult.IsVerified)
                    {
                        Response.Redirect(_settings.OnSuccessRedirectTo);
                    }
                    else
                    {
                        Response.Redirect(_settings.OnFailedRedirectTo);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        public IActionResult Error()
        {
            return View();
        }
        protected string GetStringParam(string name)
        {
            string value = this.Request.Form[name];
            return value;
        }

    }
}

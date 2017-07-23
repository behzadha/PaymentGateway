using System;
using System.Threading.Tasks;
using PaymentGateway.Model;
using PaymentGateway.ServiceReference.SamanGateway;
using System.Text;
using System.Collections.Generic;

namespace PaymentGateway.BankGatewayProvider.Saman
{
    public class SamanBankGateway : BankGateway
    {
        public SamanBankGateway(IBankGatewayConfiguration bankGatewayConfiguration) : base(bankGatewayConfiguration)
        {
        }

        public override async Task<CreateTokenResponse> CreateToken(CreateTokenRequest createTokenRequest)
        {
            PaymentGateway.ServiceReference.SamanGateway.PaymentIFBindingSoapClient client = new PaymentIFBindingSoapClient(PaymentIFBindingSoapClient.EndpointConfiguration.PaymentIFBindingSoap);
            var rs= await client.RequestTokenAsync(Configuration.TerminalId, createTokenRequest.InvoiceNumber, createTokenRequest.TotalAmount, 0, 0, 0, 0, 0, 0, null, null, 0);
            return new CreateTokenResponse
            {
                Token = rs,
                IsSuccessful = rs.Length > 3,
                OriginalErrorId=rs.Length>3 ?0 :int.Parse( rs)
            };
        }

        public override string PreparePOSTForm(string url, Dictionary<string,string> data)
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<html><body><form id=\"" + formID + "\" name=\"" + formID + "\" action=\"" + url + "\" method=\"POST\">");
            foreach (var item in data)
            {
                strForm.Append("<input type=\"hidden\" name=\"" + item.Key + "\" value=\"" + item.Value + "\">");
            }
            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." + formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script></body></html>");

            //Return the form and the script concatenated. (The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }


        public override async Task<VerifyPaymentResponse> VerifyPayment(VerifyPaymentRequest verifyRequest)
        {
            double rs = -1000;
            try
            {
                PaymentGateway.ServiceReference.SamanGateway.Payment.PaymentIFBindingSoapClient client =
                    new PaymentGateway.ServiceReference.SamanGateway.Payment.PaymentIFBindingSoapClient(
                        ServiceReference.SamanGateway.Payment.PaymentIFBindingSoapClient.EndpointConfiguration
                            .PaymentIFBindingSoap12);

                rs = await client.verifyTransactionAsync(verifyRequest.ReferenceNumber, Configuration.TerminalId);
                if (rs < 1) throw new Exception("Saman Gateway Error : Error Code = " + rs.ToString());
                return new VerifyPaymentResponse
                {
                    ReferenceNumber = verifyRequest.ReferenceNumber,
                    SaleReferenceId = verifyRequest.ReferenceNumber,
                    PaymentAmount = rs > 0 ? Convert.ToInt64(rs) : 0,
                    IsVerified = rs > 0,
                    OriginalStatus = rs.ToString()
                };

            }
            catch (Exception ex)
            {
                throw new Exception("Saman gateway Error " + rs.ToString() + " /r/n" + ex.Message);
            }
        }
        private string CreateTokenErrorMessageMapper(string rs)
        {
            switch(rs)
            {
                case "-1":
                    return "خطا در پردازش اطلاعات ارسالی";
                case "-2":
                    return "";
                case "-3":
                    return "ورودی ها حاوی کاراکتر غیر مجاز میباشد";
                case "-4":
                    return "کلمه عبور یا کد فروشنده اشتباه است";
                case "-5":
                    return "";
                case "-6":
                    return "سند قبلا برگشت کامل خورده است";
                case "-7":
                    return "رسید دیجیتالی تهی است";
                case "-8":
                    return "طول ورودی بیشتر از حد مجاز است";
                case "-9":
                    return "وجود کاراکتر غیر مجاز در مبلغ";
                case "-10":
                    return "رسید دیجیتالی به صورت Base64 نیست - حاوی کاراکتر غیر مجاز است";
                case "-11":
                    return "طول ورودی کمتر از حد مجاز است";
                case "-12":
                    return "مبلغ منفی است";
                case "-13":
                    return "";
                case "-14":
                    return "چنین تراکنشی تعریف نشده است";
                case "-15":
                    return "مبلغ به صورت اعشاری است";
                case "-16":
                    return "خطای داخلی سیستم";
                case "-17":
                    return "برگشت زدن جزیی مجاز نمی باشد";
                case "-18":
                    return "آی پی فروشنده غیر مجاز است";
                default:
                    return "";
            }
        }

        
    }

}

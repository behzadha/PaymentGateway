namespace PaymentGateway.Model
{
    public class VerifyPaymentRequest
    {
        public string MerchantId { get; set; }
        public string ReferenceNumber { get; set; }
        public string SaleReferenceId { get; set; }
    }
}

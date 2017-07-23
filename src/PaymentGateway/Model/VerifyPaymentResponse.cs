namespace PaymentGateway.Model
{
    public class VerifyPaymentResponse
    {
        public string OriginalStatus { get; set; }
        public bool IsVerified { get; set; }
        public string ReferenceNumber { get; set; }
        public string SaleReferenceId { get; set; }
        public long PaymentAmount { get; set; }
    }
}
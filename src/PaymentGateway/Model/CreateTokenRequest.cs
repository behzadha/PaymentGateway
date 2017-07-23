namespace PaymentGateway.Model
{
    public class CreateTokenRequest
    {
        public string InvoiceNumber { get; set; }
        public long TotalAmount { get; set; } 
        public string AdditionalData { get; set; }
        public string CallbackUrl { get; set; }
    }
}
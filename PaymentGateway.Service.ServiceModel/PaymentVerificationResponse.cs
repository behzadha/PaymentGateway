namespace PaymentGateway.Service.ServiceModel
{
    public class PaymentVerification
    {
        public int PaymentId { get; set; }
        public string StatusId { get; set; }
        public string ReferenceNumber { get; set; }
        public bool IsVerified { get; set; }
    }
}

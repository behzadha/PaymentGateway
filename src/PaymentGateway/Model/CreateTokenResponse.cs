namespace PaymentGateway.Model
{
    public class CreateTokenResponse
    {
        public string Token { get; set; }
        public bool IsSuccessful { get; set; }
        public int OriginalErrorId { get; set; }
        public string OriginalErrorMessage { get; set; }
    }
}
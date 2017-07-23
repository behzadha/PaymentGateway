namespace PaymentGateway.BankGatewayProvider
{
    public interface IBankGatewayConfiguration
    {
        string UserId { get; set; }
        string Password { get; set; }
        string TerminalId { get; set; }
        string GatewayUrl { get; set; }
        string CallbackUrl { get; }
        bool IsDefault { get; }
        int GatewayId { get; }
    }
}

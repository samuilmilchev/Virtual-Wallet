namespace Virtual_Wallet.Services.Contracts
{
    public interface IEmailService
    {
        Task SendAsync(string emial, string subject , string body);
    }
}

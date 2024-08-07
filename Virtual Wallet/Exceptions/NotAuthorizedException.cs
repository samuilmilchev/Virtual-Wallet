namespace Virtual_Wallet.Exceptions
{
    public class NotAuthorizedException : ApplicationException
    {
        public NotAuthorizedException(string message) : base(message)
        {

        }
    }
}

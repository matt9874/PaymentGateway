namespace PaymentGateway.Application
{
    public class BankPaymentResponse
    {
        public BankPaymentResponse(long id, bool successful)
        {
            PaymentId = id;
            Successful = successful;
        }
        public long PaymentId { get; }
        public bool Successful { get; }
    }
}

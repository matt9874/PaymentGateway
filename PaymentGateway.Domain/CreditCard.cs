namespace PaymentGateway.Domain
{
    public class CreditCard
    {
        public CreditCard(CreditCardNumber cardNumber, Month expiration, CardVerificationValue cvv)
        {
            CardNumber = cardNumber;
            Expiration = expiration;
            Cvv = cvv;
        }

        public CreditCardNumber CardNumber { get; }
        public Month Expiration { get; }
        public CardVerificationValue Cvv { get; }
    }
}

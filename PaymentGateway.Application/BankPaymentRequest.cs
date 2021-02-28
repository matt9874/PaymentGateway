using PaymentGateway.Domain;

namespace PaymentGateway.Application
{
    public class BankPaymentRequest
    {
        public BankPaymentRequest(CreditCard card, Currency currency, decimal amount, int merchantId)
        {
            Card = card;
            Currency = currency;
            Amount = amount;
            MerchantId = merchantId;
        }
        public CreditCard Card { get; }
        public Currency Currency { get; }
        public decimal Amount { get; }
        public int MerchantId { get; }
    }
}

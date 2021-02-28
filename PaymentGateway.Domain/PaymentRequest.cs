using System;

namespace PaymentGateway.Domain
{
    public class PaymentRequest
    {
        public PaymentRequest(string cardNumber, int expiryMonthNumber, int expiryYearNumber, string cvv, Currency currency, decimal amount, int merchantId)
        {
            if (amount < 0.01M)
                throw new ArgumentOutOfRangeException("amount", "Amount must be at least 0.01.");

            if(currency==Currency.None)
                throw new ArgumentOutOfRangeException("currency", "Currency is required.");

            var creditCardNumber = new CreditCardNumber(cardNumber);
            var monthOfExpiry = new Month(expiryYearNumber, expiryMonthNumber);
            var cardverificationValue = new CardVerificationValue(cvv);
            Card = new CreditCard(creditCardNumber, monthOfExpiry, cardverificationValue);
            Currency = currency;
            Amount = amount;
            MerchantId = merchantId;
        }
        public CreditCard Card { get; }
        public Currency Currency { get; set; }
        public decimal Amount { get; set; }
        public int MerchantId { get; set; }
    }
}

using PaymentGateway.Application.Interfaces;
using PaymentGateway.Domain;

namespace PaymentGateway.Application
{
    public class PaymentRequestMapper : IMapper<PaymentRequest, BankPaymentRequest>
    {
        public BankPaymentRequest Map(PaymentRequest input)
        {
            return new BankPaymentRequest(input.Card, input.Currency, input.Amount, input.MerchantId);
        }
    }
}

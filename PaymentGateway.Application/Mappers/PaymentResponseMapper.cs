using PaymentGateway.Application.Interfaces;
using PaymentGateway.Domain;

namespace PaymentGateway.Application
{
    public class PaymentResponseMapper : IMapper<BankPaymentResponse, PaymentResult>
    {
        public PaymentResult Map(BankPaymentResponse bankResponse)
        {
            return new PaymentResult(bankResponse.PaymentId, bankResponse.Successful);
        }
    }
}

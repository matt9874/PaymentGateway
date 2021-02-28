using PaymentGateway.Domain;
using System.Threading.Tasks;

namespace PaymentGateway.Application.Interfaces
{
    public interface IProcessPaymentService
    {
        Task<PaymentResult> ProcessPayment(PaymentRequest paymentRequest);
    }
}

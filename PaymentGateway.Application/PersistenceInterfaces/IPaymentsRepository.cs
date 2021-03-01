using PaymentGateway.Domain;
using System.Threading.Tasks;

namespace PaymentGateway.Application.PersistenceInterfaces
{
    public interface IPaymentsRepository
    {
        Task SavePayment(PaymentRequest payment);
        Task<PaymentRequest> GetPaymentForMerchant(int merchantId, long paymentId);
    }
}

using System.Threading.Tasks;

namespace PaymentGateway.Application.Interfaces
{
    public interface IBankClient
    {
        Task<BankPaymentResponse> ChargePayer(BankPaymentRequest bankPaymentRequest);
    }
}

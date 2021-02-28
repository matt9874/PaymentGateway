using PaymentGateway.Domain;
using System.Threading.Tasks;

namespace PaymentGateway.Application.PersistenceInterfaces
{
    public interface IMerchantsRepository
    {
        Task<Merchant> ReadMerchant(int id);
    }
}

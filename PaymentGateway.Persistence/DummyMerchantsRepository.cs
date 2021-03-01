using PaymentGateway.Application.PersistenceInterfaces;
using PaymentGateway.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.Persistence
{
    public class DummyMerchantsRepository : IMerchantsRepository
    {
        private static readonly Dictionary<int, Merchant> _merchants = new Dictionary<int, Merchant>()
        {
            {1, new Merchant(){Id=1, Name = "Merchant A" } },
            {2, new Merchant(){Id=1, Name = "Merchant B" } },
            {3, new Merchant(){Id=1, Name = "Merchant C" } }
        };

        public Task<Merchant> ReadMerchant(int id)
        {
            if (_merchants.ContainsKey(id))
                return Task.FromResult(_merchants[id]);
            return Task.FromResult((Merchant)null);
        }
    }
}

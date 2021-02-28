using System.Collections.Generic;

namespace PaymentGateway.Domain
{
    public class Merchant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<PaymentRequest> Payments { get; set; }
    }
}

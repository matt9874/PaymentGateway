using System;

namespace PaymentGateway.Domain
{
    public class PaymentResult
    {
        public PaymentResult(long? id, bool successful)
        {
            PaymentId = id;
            Successful = successful;
        }
        public long? PaymentId { get; }
        public bool Successful { get; }
    }
}

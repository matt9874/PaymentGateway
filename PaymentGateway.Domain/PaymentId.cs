using System;

namespace PaymentGateway.Domain
{
    public struct PaymentId: IEquatable<PaymentId>
    {
        public PaymentId(long value)
        {
            Value = value;
        }
        public long Value { get; }

        public override bool Equals(object obj)
        {
            if (obj is PaymentId otherPaymentId)
                return Equals(otherPaymentId);

            return false;
        }

        public bool Equals(PaymentId other)
        {
            return Value == other.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}

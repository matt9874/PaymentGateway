using PaymentGateway.Domain.Extensions;
using System;

namespace PaymentGateway.Domain
{
    public class CardVerificationValue
    {
        public CardVerificationValue(string value)
        {
            if(value == null)
                throw new ArgumentNullException("value");

            if (value.Length !=3)
                throw new ArgumentOutOfRangeException("value","cvv number must have three characters");

            if (!value.IsInteger())
                throw new ArgumentOutOfRangeException("value", "must be possible to convert CVV to an integer");

            Value = value;
        }

        public string Value { get; }
    }
}
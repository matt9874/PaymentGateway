using System;
using System.Linq;

namespace PaymentGateway.Domain
{
    public struct CreditCardNumber: IEquatable<CreditCardNumber>
    {
        private static readonly string _maskPrefix = "XXXXXXXXXXXX";

        public CreditCardNumber(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.Length<13 || value.Length > 16)
            {
                throw new ArgumentOutOfRangeException(
                    "value",
                    "Credit card number must have between 13 and 16 digits.");
            }
            if (!value.All(c => char.IsDigit(c)))
            {
                throw new ArgumentOutOfRangeException(
                    "value",
                    "Credit card number can contain only numeric characters.");
            }
            Value = value;
        }

        public string Value { get; }

        public override bool Equals(Object obj)
        {
            if (obj is CreditCardNumber otherCreditCardNumber)
                return Equals(otherCreditCardNumber);
            return false;
        }

        public bool Equals(CreditCardNumber other)
        {
            return Value.Equals(other.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            string lastFourDigitsText = Value.Substring(12, 4);
            return _maskPrefix + lastFourDigitsText;
        }
    }
}
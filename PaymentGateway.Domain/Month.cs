using System;

namespace PaymentGateway.Domain
{
    public struct Month
    {
        public Month(int yearAd, int monthOfTheYear)
        {
            if (!Enum.IsDefined(typeof(MonthOfTheYear), monthOfTheYear) || monthOfTheYear == 0)
                throw new ArgumentOutOfRangeException("monthOfTheYear", "monthOfTheYear must be between 1 and 12");

            YearAd = yearAd;
            MonthOfYear = (MonthOfTheYear)monthOfTheYear;
        }

        public int YearAd { get; }
        public MonthOfTheYear MonthOfYear { get; }

        public override bool Equals(object obj)
        {
            if (obj is Month otherMonth)
                return Equals(otherMonth);
            return false;
        }

        public bool Equals(Month other)
        {
            return YearAd == other.YearAd && MonthOfYear == other.MonthOfYear;
        }

        public override int GetHashCode()
        {
            return (YearAd, MonthOfYear).GetHashCode();
        }
    }
}

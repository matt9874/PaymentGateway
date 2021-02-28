using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PaymentGateway.Domain.Tests
{
    [TestClass]
    public class MonthTests
    {
        [TestMethod]
        [DataRow(0)]
        [DataRow(13)]
        public void ctor_InvalidMonthNumber_ThrowsArgumentOutOfRangeException(int monthNumber)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new Month(2022, monthNumber));
        }
    }
}

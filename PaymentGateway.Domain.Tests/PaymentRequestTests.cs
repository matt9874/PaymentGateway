using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PaymentGateway.Domain.Tests
{
    [TestClass]
    public class PaymentRequestTests
    {
        [TestMethod]
        public void ctor_UnknownCurrency_ThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PaymentRequest("1234512345123456", 1, 2022, "123", Currency.None, 1.0M, 1));
        }
        [TestMethod]
        [DataRow(0.00999)]
        [DataRow(0)]
        [DataRow(-1)]
        public void ctor_InvalidAmount_ThrowsArgumentOutOfRangeException(double amount)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new PaymentRequest("1234512345123456", 1, 2022, "123", Currency.Euro, (decimal)amount, 1));
        }
        
    }
}

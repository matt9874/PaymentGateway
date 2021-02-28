using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace PaymentGateway.Domain.Tests
{
    [TestClass]
    public class CardVerificationValueTests
    {
        [TestMethod]
        public void ctor_Null_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new CardVerificationValue(null));
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("abc")]
        [DataRow("a23")]
        [DataRow("1b3")]
        [DataRow("12c")]
        [DataRow("1234")]
        [DataRow("12")]
        public void ctor_InvalidCvv_ThrowsArgumentNOutOfRangeException(string cvv)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new CardVerificationValue(cvv));
        }
    }
}

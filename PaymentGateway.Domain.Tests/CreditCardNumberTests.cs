using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace PaymentGateway.Domain.Tests
{
    [TestClass]
    public class CreditCardNumberTests
    {
        [TestMethod]
        public void ctor_NullInput_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new CreditCardNumber(null));
        }

        [TestMethod]
        [DataRow("-1")]
        [DataRow("10000000000000000")]
        [DataRow("10000000000000000b")]
        [DataRow("1000000000000000b")]
        public void ctor_InputOutOfRange_ThrowsArgumentOutOfRangeException(string number)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(
                () => new CreditCardNumber(number));
        }

        [TestMethod]
        [DataRow("0000000000000000")]
        [DataRow("0000000000001000")]
        [DataRow("0000000010000000")]
        [DataRow("0000100000000000")]
        [DataRow("1000000000000000")]
        [DataRow("9999999999999999")]
        public void Value_InputInRange_NumberHasCorrectValue(string number)
        {
            var cardNumber = new CreditCardNumber(number);
            Assert.AreEqual(number, cardNumber.Value);
        }

        [TestMethod]
        [DataRow("0000000000000000", "0000")]
        [DataRow("0000000000000005", "0005")]
        [DataRow("0000000000000050", "0050")]
        [DataRow("0000000000000500", "0500")]
        [DataRow("0000000000005000", "5000")]
        [DataRow("0000000000010000", "0000")]
        [DataRow("1000000000000000", "0000")]
        [DataRow("9999999999999999", "9999")]
        public void ToString_InputInRange_LastFourCharactersAreCorrect(string number, string expectedLastFourCharacters)
        {
            var cardNumber = new CreditCardNumber(number);
            string cardNumberText = cardNumber.ToString();
            string lastFourCharacters = cardNumberText.Substring(cardNumberText.Length - 4, 4);
            Assert.AreEqual(expectedLastFourCharacters, lastFourCharacters);
        }

        [TestMethod]
        [DataRow("0000000000000000")]
        [DataRow("0000000000000005")]
        [DataRow("0000000000000050")]
        [DataRow("0000000000000500")]
        [DataRow("0000000000005000")]
        [DataRow("0000000000010000")]
        [DataRow("1000000000000000")]
        [DataRow("9999999999999999")]
        public void ToString_InputInRange_CharactersBeforeLastFourAreNonNumeric(string number)
        {
            var cardNumber = new CreditCardNumber(number);
            string cardNumberText = cardNumber.ToString();
            string startingCharacters = cardNumberText.Substring(0, cardNumberText.Length - 4);
            Assert.IsTrue(startingCharacters.All(c => !char.IsDigit(c)));
        }
    }
}

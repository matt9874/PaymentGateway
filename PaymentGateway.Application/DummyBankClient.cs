using PaymentGateway.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Application
{
    public class DummyBankClient: IBankClient
    {
        private static long? _nextPaymentId = 1L;
        private static readonly Random _random = new Random();

        public Task<BankPaymentResponse> ChargePayer(BankPaymentRequest bankPaymentRequest)
        {
            int randomSingleDigitInteger = _random.Next(1,9);
            if (randomSingleDigitInteger != 9)
            {
                var response = new BankPaymentResponse(_nextPaymentId, true);
                _nextPaymentId++;
                return Task.FromResult(response);
            }
            return Task.FromResult(new BankPaymentResponse(null, false));
        }
    }
}

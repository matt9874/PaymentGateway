using PaymentGateway.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace PaymentGateway.Application
{
    public class DummyBankClient: IBankClient
    {
        private static long _nextPaymentId = 1L;
        private static readonly Random _random = new Random();

        public Task<BankPaymentResponse> ChargePayer(BankPaymentRequest bankPaymentRequest)
        {
            int randomSingleDigitInteger = _random.Next(1,9);

            BankPaymentResponse response;
            if (randomSingleDigitInteger != 9)
                response = new BankPaymentResponse(_nextPaymentId, true);
            else
                response = new BankPaymentResponse(_nextPaymentId, false);
            
            _nextPaymentId++;
            return Task.FromResult(response);
        }
    }
}

using PaymentGateway.Application.PersistenceInterfaces;
using PaymentGateway.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PaymentGateway.Persistence
{
    public class DummyPaymentsRepository : IPaymentsRepository
    {
        private static readonly Dictionary<long, PaymentRequest> _paymentRequests = new Dictionary<long, PaymentRequest>();

        public Task SavePayment(PaymentRequest payment)
        {
            if (payment.Id == null)
                throw new ArgumentNullException();
            if (_paymentRequests.ContainsKey(payment.Id.Value))
                throw new InvalidOperationException("Payment already exists");
            _paymentRequests[payment.Id.Value] = payment;
            return Task.CompletedTask;
        }
    }
}

using PaymentGateway.API.Models;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Domain;
using System;

namespace PaymentGateway.API.Mappers
{
    public class PaymentDetailsMapper : IMapper<PaymentRequest, PaymentDetailsDto>
    {
        public PaymentDetailsDto Map(PaymentRequest paymentRequest)
        {
            if (!paymentRequest.Successful.HasValue)
                throw new ArgumentOutOfRangeException("paymentRequest", "Successful property of paymentRequest should be true or false");

            if (!paymentRequest.Id.HasValue)
                throw new ArgumentOutOfRangeException("paymentRequest", "Id property of paymentRequest should be true or false");

            return new PaymentDetailsDto()
            {
                MaskedCardNumber = paymentRequest.Card.CardNumber.ToString(),
                Currency = paymentRequest.Currency.ToString(),
                Amount = paymentRequest.Amount,
                MerchantId = paymentRequest.MerchantId,
                Successful = paymentRequest.Successful.Value,
                Id = paymentRequest.Id.Value
            };
        }
    }
}

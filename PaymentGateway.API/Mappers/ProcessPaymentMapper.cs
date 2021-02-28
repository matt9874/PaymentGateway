using PaymentGateway.API.Models;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Domain;
using System;

namespace PaymentGateway.API.Mappers
{
    public class ProcessPaymentMapper : IMapper<(int merchantId, ProcessPaymentDto processPaymentDto), PaymentRequest>
    {
        public PaymentRequest Map((int merchantId, ProcessPaymentDto processPaymentDto) input)
        {
            if (!Enum.TryParse(input.processPaymentDto.Currency, out Currency currency))
                throw new ArgumentOutOfRangeException("Currency");

            return new PaymentRequest(
                input.processPaymentDto.CardNumber, 
                input.processPaymentDto.ExpiryMonth, 
                input.processPaymentDto.ExpiryYear,
                input.processPaymentDto.Cvv,
                currency,
                input.processPaymentDto.Amount,
                input.merchantId);
        }
    }
}

using PaymentGateway.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.API.ValidationAttributes
{
    public class StringToCurrencyValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (!(value is string currencyText))
                return new ValidationResult("Currency string required");

            if (!Enum.TryParse<Currency>(currencyText, out Currency currency) || currency == Domain.Currency.None)
                return new ValidationResult("Invalid currency");

            return ValidationResult.Success;
        }
    }
}

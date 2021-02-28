using PaymentGateway.Domain.Extensions;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.API.ValidationAttributes
{
    public class CvvValidationAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (!(value is string cvvText))
                return new ValidationResult("Cvv string required");

            if (cvvText.Length!=3)
                return new ValidationResult("Cvv must have three characters");

            if (!cvvText.IsInteger())
                return new ValidationResult("Cvv must have three characters");

            return ValidationResult.Success;
        }
    }
}

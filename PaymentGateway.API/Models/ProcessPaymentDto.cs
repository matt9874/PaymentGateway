using PaymentGateway.API.ValidationAttributes;
using PaymentGateway.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.API.Models
{
    public class ProcessPaymentDto
    {
        [Required(ErrorMessage = "Credit card number is required.")]
        [CreditCard(ErrorMessage = "Invalid credit card number.")]
        public string CardNumber { get; set; }

        [Required(ErrorMessage = "Expiry month is required.")]
        [Range(1,12, ErrorMessage = "Expiry month must be a number between 1 and 12.")]
        public int ExpiryMonth { get; set; }

        [Required(ErrorMessage = "Expiry year is required")]
        [Range(2000, int.MaxValue, ErrorMessage = "Invalid expiry year.")]
        public int ExpiryYear { get; set; }

        [Required(ErrorMessage = "Cvv is required.")]
        [CvvValidation]
        public string Cvv { get; set; }

        [StringToCurrencyValidation]
        public string Currency { get; set; }

        [Required(ErrorMessage ="Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be at least 0.01.")]
        public decimal Amount { get; set; }
    }
}

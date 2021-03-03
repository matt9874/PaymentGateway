using PaymentGateway.API.ValidationAttributes;
using PaymentGateway.Domain;
using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.API.Models
{
    /// <summary>
    /// Payment information
    /// </summary>
    public class ProcessPaymentDto
    {
        /// <summary>
        /// A valid credit card number
        /// </summary>
        [Required(ErrorMessage = "Credit card number is required.")]
        [CreditCard(ErrorMessage = "Invalid credit card number.")]
        public string CardNumber { get; set; }

        /// <summary>
        /// Month of expiry
        /// </summary>
        [Required(ErrorMessage = "Expiry month is required.")]
        [Range(1,12, ErrorMessage = "Expiry month must be a number between 1 and 12.")]
        public int ExpiryMonth { get; set; }

        /// <summary>
        /// Year of expiry
        /// </summary>
        [Required(ErrorMessage = "Expiry year is required")]
        [Range(2000, int.MaxValue, ErrorMessage = "Invalid expiry year.")]
        public int ExpiryYear { get; set; }

        /// <summary>
        /// A valid CVV
        /// </summary>
        [Required(ErrorMessage = "Cvv is required.")]
        [CvvValidation]
        public string Cvv { get; set; }

        /// <summary>
        /// A supported currency: Euro or PoundsSterling
        /// </summary>
        [StringToCurrencyValidation]
        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; }

        /// <summary>
        /// Amount of money being paid
        /// </summary>
        [Required(ErrorMessage ="Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be at least 0.01.")]
        public decimal Amount { get; set; }
    }
}

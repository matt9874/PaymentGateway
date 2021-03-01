namespace PaymentGateway.API.Models
{
    public class PaymentDetailsDto
    {
        public string MaskedCardNumber { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public int MerchantId { get; set; }
        public bool Successful { get; set; }
        public long Id { get; set; }
    }
}

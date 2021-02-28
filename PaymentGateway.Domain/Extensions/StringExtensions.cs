namespace PaymentGateway.Domain.Extensions
{
    public static class StringExtensions
    {
        public static bool IsInteger(this string text)
        {
            return int.TryParse(text, out _);
        }
    }
}

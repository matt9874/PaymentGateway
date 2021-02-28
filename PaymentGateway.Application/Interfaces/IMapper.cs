namespace PaymentGateway.Application.Interfaces
{
    public interface IMapper<TIn, TOut>
    {
        TOut Map(TIn input);
    }
}

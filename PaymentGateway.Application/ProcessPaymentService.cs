using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.PersistenceInterfaces;
using PaymentGateway.Domain;
using System.Threading.Tasks;

namespace PaymentGateway.Application
{
    public class ProcessPaymentService : IProcessPaymentService
    {
        private readonly IMapper<PaymentRequest, BankPaymentRequest> _paymentRequestMapper;
        private readonly IBankClient _bankClient;
        private readonly IMapper<BankPaymentResponse, PaymentResult> _paymentResponseMapper;
        private readonly IPaymentsRepository _paymentsRepository;

        public ProcessPaymentService(IMapper<PaymentRequest, BankPaymentRequest> paymentRequestMapper, 
            IBankClient bankClient, IMapper<BankPaymentResponse, PaymentResult> paymentResponseMapper,
            IPaymentsRepository paymentsRepository)
        {
            _paymentRequestMapper = paymentRequestMapper;
            _bankClient = bankClient;
            _paymentResponseMapper = paymentResponseMapper;
            _paymentsRepository = paymentsRepository;
        }

        public async Task<PaymentResult> ProcessPayment(PaymentRequest paymentRequest)
        {
            BankPaymentRequest bankPaymentRequest = _paymentRequestMapper.Map(paymentRequest);
            BankPaymentResponse bankPaymentResponse = await _bankClient.ChargePayer(bankPaymentRequest);
            PaymentResult paymentResult = _paymentResponseMapper.Map(bankPaymentResponse);

            paymentRequest.Successful = paymentResult.Successful;
            paymentRequest.Id = paymentResult.PaymentId;

            await _paymentsRepository.SavePayment(paymentRequest);

            return paymentResult;
        }
    }
}

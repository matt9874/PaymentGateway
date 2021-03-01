using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Models;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.PersistenceInterfaces;
using PaymentGateway.Domain;
using System.Threading.Tasks;

namespace PaymentGateway.API.Controllers
{
    [Route("api/merchants/{merchantId}/payments")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMapper<(int, ProcessPaymentDto), PaymentRequest> _processPaymentMapper;
        private readonly IMerchantsRepository _merchantRepository;
        private readonly IProcessPaymentService _processPaymentService;
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly IMapper<PaymentRequest, PaymentDetailsDto> _paymentDetailsMapper;

        public PaymentsController(IMapper<(int, ProcessPaymentDto), PaymentRequest> processPaymentMapper,
            IMerchantsRepository merchantRepository, IProcessPaymentService processPaymentService,
            IPaymentsRepository paymentsRepository, IMapper<PaymentRequest, PaymentDetailsDto> paymentDetailsMapper)
        {
            _processPaymentMapper = processPaymentMapper;
            _merchantRepository = merchantRepository;
            _processPaymentService = processPaymentService;
            _paymentsRepository = paymentsRepository;
            _paymentDetailsMapper = paymentDetailsMapper;
        }

        [HttpGet("{paymentId}", Name = "GetPaymentDetails")]
        public async Task<IActionResult> GetPaymentDetails([FromRoute] int merchantId, [FromRoute] long paymentId)
        {
            PaymentRequest paymentRequest = await _paymentsRepository.GetPaymentForMerchant(merchantId, paymentId);

            if (paymentRequest == null)
                return NotFound();

            PaymentDetailsDto paymentDetailsDto = _paymentDetailsMapper.Map(paymentRequest);
            return Ok(paymentDetailsDto);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessNewPayment([FromRoute] int merchantId,
            [FromBody] ProcessPaymentDto processPaymentDto)
        {
            Merchant merchant = await _merchantRepository.ReadMerchant(merchantId);
            if (merchant is null)
                return NotFound();

            PaymentRequest paymentRequest = _processPaymentMapper.Map((merchantId, processPaymentDto));
            PaymentResult paymentResult = await _processPaymentService.ProcessPayment(paymentRequest);
            PaymentDetailsDto paymentDetails = _paymentDetailsMapper.Map(paymentRequest);

            return CreatedAtRoute("GetPaymentDetails",
                new
                {
                    merchantId = merchantId,
                    paymentId = paymentResult.PaymentId
                },
                paymentDetails);
        }
    }
}

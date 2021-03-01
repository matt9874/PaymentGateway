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

        public PaymentsController(IMapper<(int, ProcessPaymentDto), PaymentRequest> processPaymentMapper,
            IMerchantsRepository merchantRepository, IProcessPaymentService processPaymentService)
        {
            _processPaymentMapper = processPaymentMapper;
            _merchantRepository = merchantRepository;
            _processPaymentService = processPaymentService;
        }

        [HttpGet("{paymentId}", Name = "GetPaymentDetails")]
        public async Task<IActionResult> GetPaymentDetails([FromRoute] int merchantId, [FromRoute] long paymentId)
        {
            return NotFound();
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

            if (paymentResult.Successful)
            {
                return CreatedAtRoute("GetPaymentDetails",
                    new
                    {
                        merchantId = merchantId,
                        paymentId = paymentResult.PaymentId
                    },
                    paymentRequest);
            }
            return Conflict("Payment was unsuccessful.");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using PaymentGateway.API.Models;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.PersistenceInterfaces;
using PaymentGateway.Domain;
using System.Collections.Generic;
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
            paymentDetailsDto.Links.AddRange(CreateLinksForPayment(merchantId, paymentDetailsDto.Id));
            
            return Ok(paymentDetailsDto);
        }

        [HttpPost(Name = "ProcessNewPayment")]
        public async Task<IActionResult> ProcessNewPayment([FromRoute] int merchantId,
            [FromBody] ProcessPaymentDto processPaymentDto)
        {
            throw new System.Exception("XXX_XXX_XXX_XXX");
            Merchant merchant = await _merchantRepository.ReadMerchant(merchantId);
            if (merchant is null)
                return NotFound();

            PaymentRequest paymentRequest = _processPaymentMapper.Map((merchantId, processPaymentDto));
            PaymentResult paymentResult = await _processPaymentService.ProcessPayment(paymentRequest);
            PaymentDetailsDto paymentDetails = _paymentDetailsMapper.Map(paymentRequest);

            paymentDetails.Links.AddRange(CreateLinksForPayment(merchantId, paymentDetails.Id));

            return CreatedAtRoute("GetPaymentDetails",
                new
                {
                    merchantId = merchantId,
                    paymentId = paymentResult.PaymentId
                },
                paymentDetails);
        }

        private IEnumerable<LinkDto> CreateLinksForPayment(int merchantId, long paymentId)
        {
            return new LinkDto[]
            {
                new LinkDto(Url.Link("GetPaymentDetails", new {merchantId, paymentId }),
                            "self",
                            "GET")
            };
        }
    }
}

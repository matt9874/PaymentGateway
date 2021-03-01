using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.API.Controllers;
using PaymentGateway.API.Models;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.PersistenceInterfaces;
using PaymentGateway.Domain;
using System.Threading.Tasks;

namespace PaymentGateway.API.Tests.ControllersTests
{
    [TestClass]
    public class PaymentsControllerTests
    {
        private Mock<IMapper<(int, ProcessPaymentDto), PaymentRequest>> _mockMapper;
        private Mock<IMerchantsRepository> _mockMerchantRepository;
        private Mock<IProcessPaymentService> _mockProcessPaymentService;
        private Mock<IPaymentsRepository> _mockPaymentsRepository;
        private Mock<IMapper<PaymentRequest, PaymentDetailsDto>> _mockPaymentDetailsMapper;
        private PaymentsController _paymentsController;

        [TestInitialize]
        public void TestInit()
        {
            _mockMapper = new Mock<IMapper<(int, ProcessPaymentDto), PaymentRequest>>();
            _mockMerchantRepository = new Mock<IMerchantsRepository>();
            _mockProcessPaymentService = new Mock<IProcessPaymentService>();
            _mockPaymentsRepository = new Mock<IPaymentsRepository>();
            _mockPaymentDetailsMapper = new Mock<IMapper<PaymentRequest, PaymentDetailsDto>>();
            _paymentsController = new PaymentsController(_mockMapper.Object, _mockMerchantRepository.Object, 
                _mockProcessPaymentService.Object, _mockPaymentsRepository.Object, _mockPaymentDetailsMapper.Object);
        }

        [TestMethod]
        public async Task ProcessNewPayment_NoMerchantInStorage_ReturnsNotFound()
        {
            _mockMerchantRepository.Setup(r => r.ReadMerchant(1))
                .ReturnsAsync((Merchant)null);

            var postResult = await _paymentsController.ProcessNewPayment(1, new ProcessPaymentDto());

            Assert.IsInstanceOfType(postResult, typeof(NotFoundResult));
        }
        
        [TestMethod]
        public async Task ProcessNewPayment_PaymentServiceIsSuccessful_ReturnsCreatedAtRouteResult()
        {
            _mockMerchantRepository.Setup(r => r.ReadMerchant(1))
                .ReturnsAsync(new Merchant() { Id = 1 });
            _mockProcessPaymentService.Setup(ps => ps.ProcessPayment(It.IsAny<PaymentRequest>()))
                .ReturnsAsync(new PaymentResult(1L, true));

            var postResult = await _paymentsController.ProcessNewPayment(1, new ProcessPaymentDto());

            Assert.IsInstanceOfType(postResult, typeof(CreatedAtRouteResult));
        }

        [TestMethod]
        public async Task ProcessNewPayment_PaymentServiceIsSuccessful_RouteNameEqualsGetPaymentDetails()
        {
            _mockMerchantRepository.Setup(r => r.ReadMerchant(1))
                .ReturnsAsync(new Merchant() { Id = 1 });
            _mockProcessPaymentService.Setup(ps => ps.ProcessPayment(It.IsAny<PaymentRequest>()))
                .ReturnsAsync(new PaymentResult(1L, true));

            var postResult = await _paymentsController.ProcessNewPayment(1, new ProcessPaymentDto());

            var createdAtRouteResult = (CreatedAtRouteResult)postResult;
            Assert.AreEqual("GetPaymentDetails", createdAtRouteResult.RouteName);
        }

        [TestMethod]
        [DataRow("merchantId")]
        [DataRow("paymentId")]
        public async Task ProcessNewPayment_PaymentServiceIsSuccessful_RouteValuesContainsKey(string propertyName)
        {
            int merchantId = 1;

            _mockMerchantRepository.Setup(r => r.ReadMerchant(merchantId))
                .ReturnsAsync(new Merchant() { Id = merchantId });
            _mockProcessPaymentService.Setup(ps => ps.ProcessPayment(It.IsAny<PaymentRequest>()))
                .ReturnsAsync(new PaymentResult(1L, true));

            var postResult = await _paymentsController.ProcessNewPayment(merchantId, new ProcessPaymentDto());
            var createdAtRouteResult = (CreatedAtRouteResult)postResult;

            Assert.IsTrue(createdAtRouteResult.RouteValues.ContainsKey(propertyName));
        }

        [TestMethod]
        [DataRow("merchantId", 2)]
        [DataRow("paymentId", 1L)]
        public async Task ProcessNewPayment_PaymentServiceIsSuccessful_RouteValuePropertyHasCorrectValue(string propertyName, object expectedRouteValue)
        {
            int merchantId = 2;
            long paymentId = 1L;

            _mockMerchantRepository.Setup(r => r.ReadMerchant(merchantId))
                .ReturnsAsync(new Merchant() { Id = merchantId });
            _mockProcessPaymentService.Setup(ps => ps.ProcessPayment(It.IsAny<PaymentRequest>()))
                .ReturnsAsync(new PaymentResult(paymentId, true));

            var postResult = await _paymentsController.ProcessNewPayment(merchantId, new ProcessPaymentDto());
            var createdAtRouteResult = (CreatedAtRouteResult)postResult;

            Assert.AreEqual(expectedRouteValue, createdAtRouteResult.RouteValues[propertyName]);
        }

        [TestMethod]
        public async Task ProcessNewPayment_PaymentServiceIsUnsuccessful_ReturnsConflict()
        {
            int merchantId = 2;
            _mockMerchantRepository.Setup(r => r.ReadMerchant(merchantId))
                .ReturnsAsync(new Merchant() { Id = merchantId });
            _mockProcessPaymentService.Setup(ps => ps.ProcessPayment(It.IsAny<PaymentRequest>()))
                .ReturnsAsync(new PaymentResult(1L, false));

            var postResult = await _paymentsController.ProcessNewPayment(merchantId, new ProcessPaymentDto());

            Assert.IsInstanceOfType(postResult, typeof(ConflictObjectResult));
        }

        [TestMethod]
        public async Task GetPaymentDetails_NoPaymentFromRepository_ReturnsNotFound()
        {
            _mockPaymentsRepository.Setup(r => r.GetPaymentForMerchant(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync((PaymentRequest)null);

            var postResult = await _paymentsController.GetPaymentDetails(1, 2L);

            Assert.IsInstanceOfType(postResult, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetPaymentDetails_MapperReturnsPaymentDetailsDto_ReturnsOk()
        {
            _mockPaymentsRepository.Setup(r => r.GetPaymentForMerchant(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(new PaymentRequest("1234512345123456", 1, 2222, "111", Currency.Euro, 1M, 1));

            PaymentDetailsDto paymentDetails = new PaymentDetailsDto();
            _mockPaymentDetailsMapper.Setup(m => m.Map(It.IsAny<PaymentRequest>()))
                .Returns(paymentDetails);

            var postResult = await _paymentsController.GetPaymentDetails(1, 2L);

            Assert.IsInstanceOfType(postResult, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetPaymentDetails_MapperReturnsPaymentDetailsDto_MappedPaymentDetailsInBody()
        {
            _mockPaymentsRepository.Setup(r => r.GetPaymentForMerchant(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync(new PaymentRequest("1234512345123456", 1, 2222, "111", Currency.Euro, 1M, 1));

            PaymentDetailsDto paymentDetails = new PaymentDetailsDto();
            _mockPaymentDetailsMapper.Setup(m => m.Map(It.IsAny<PaymentRequest>()))
                .Returns(paymentDetails);

            var postResult = await _paymentsController.GetPaymentDetails(1, 2L);
            var okObjectResult = (OkObjectResult)postResult;

            Assert.AreEqual(paymentDetails, okObjectResult.Value);
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PaymentGateway.Application.Interfaces;
using PaymentGateway.Application.PersistenceInterfaces;
using PaymentGateway.Domain;
using System.Threading.Tasks;

namespace PaymentGateway.Application.Tests
{
    [TestClass]
    public class ProcessPaymentServiceTests
    {
        private Mock<IMapper<PaymentRequest, BankPaymentRequest>> _mockRequestMapper;
        private Mock<IBankClient> _mockBankClient;
        private Mock<IMapper<BankPaymentResponse, PaymentResult>> _mockPaymentResponseMapper;
        private Mock<IPaymentsRepository> _mockPaymentsRepository;
        private ProcessPaymentService _processPaymentService;

        [TestInitialize]
        public void TestInit()
        {
            _mockRequestMapper = new Mock<IMapper<PaymentRequest, BankPaymentRequest>>();
            _mockBankClient = new Mock<IBankClient>();
            _mockPaymentResponseMapper= new Mock<IMapper<BankPaymentResponse, PaymentResult>>();
            _mockPaymentsRepository = new Mock<IPaymentsRepository>();
            _processPaymentService = new ProcessPaymentService(_mockRequestMapper.Object, _mockBankClient.Object,
                _mockPaymentResponseMapper.Object, _mockPaymentsRepository.Object);
        }

        [TestMethod]
        public async Task ProcessPayment_ResponseSuccessful_RequestSetToSuccessful()
        {
            var request = new PaymentRequest("1234512345123456", 1, 2222, "111", Currency.Euro, 1M, 1);
            _mockPaymentResponseMapper.Setup(m => m.Map(It.IsAny<BankPaymentResponse>()))
                .Returns(new PaymentResult(1L, true));

            await _processPaymentService.ProcessPayment(request);
            Assert.IsTrue(request.Successful.Value);
        }

        [TestMethod]
        public async Task ProcessPayment_ResponseSuccessful_RequestIsSavedOnce()
        {
            var request = new PaymentRequest("1234512345123456", 1, 2222, "111", Currency.Euro, 1M, 1);
            _mockPaymentResponseMapper.Setup(m => m.Map(It.IsAny<BankPaymentResponse>()))
                .Returns(new PaymentResult(1L, true));

            await _processPaymentService.ProcessPayment(request);
            _mockPaymentsRepository.Verify(r => r.SavePayment(request), Times.Once);
        }

        [TestMethod]
        public async Task ProcessPayment_ResponseSuccessful_SuccessfulResponseReturned()
        {
            var request = new PaymentRequest("1234512345123456", 1, 2222, "111", Currency.Euro, 1M, 1);
            var expectedResult = new PaymentResult(1L, true);
            _mockPaymentResponseMapper.Setup(m => m.Map(It.IsAny<BankPaymentResponse>()))
                .Returns(expectedResult);

            var result = await _processPaymentService.ProcessPayment(request);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public async Task ProcessPayment_ResponseUnsuccessful_RequestSetToUnsuccessful()
        {
            var request = new PaymentRequest("1234512345123456", 1, 2222, "111", Currency.Euro, 1M, 1);
            _mockPaymentResponseMapper.Setup(m => m.Map(It.IsAny<BankPaymentResponse>()))
                .Returns(new PaymentResult(1L, false));

            await _processPaymentService.ProcessPayment(request);
            Assert.IsFalse(request.Successful.Value);
        }

        [TestMethod]
        public async Task ProcessPayment_ResponseUnsuccessful_RequestIsSaved()
        {
            var request = new PaymentRequest("1234512345123456", 1, 2222, "111", Currency.Euro, 1M, 1);
            _mockPaymentResponseMapper.Setup(m => m.Map(It.IsAny<BankPaymentResponse>()))
                .Returns(new PaymentResult(1L, false));

            await _processPaymentService.ProcessPayment(request);
            _mockPaymentsRepository.Verify(r => r.SavePayment(request), Times.Once);
        }

        [TestMethod]
        public async Task ProcessPayment_ResponseUnsuccessful_UnsuccessfulResponseReturned()
        {
            var request = new PaymentRequest("1234512345123456", 1, 2222, "111", Currency.Euro, 1M, 1);
            var expectedResult = new PaymentResult(1L, false);
            _mockPaymentResponseMapper.Setup(m => m.Map(It.IsAny<BankPaymentResponse>()))
                .Returns(expectedResult);

            var result = await _processPaymentService.ProcessPayment(request);

            Assert.AreEqual(expectedResult, result);
        }
    }
}

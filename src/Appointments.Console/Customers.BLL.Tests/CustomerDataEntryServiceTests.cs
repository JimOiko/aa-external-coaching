using AppointmentManagementSystem.DomainObjects.Interfaces;
using Customers.BLL.Interfaces;
using Moq;
using Moq.Protected;

namespace Customers.BLL.Tests
{
    public class CustomerDataEntryServiceTests
    {
        private readonly Mock<IUserInputService> _userInputServiceMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly ICustomerDataEntryService _customerDataEntryService;

        public CustomerDataEntryServiceTests()
        {
            _userInputServiceMock = new Mock<IUserInputService>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _customerDataEntryService = new CustomerDataEntryService(_httpClient, _userInputServiceMock.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Customer()
        {
            // Arrange
            _userInputServiceMock.SetupSequence(u => u.ReadLine())
                .Returns("John Doe")       // Name
                .Returns("john@example.com") // Email
                .Returns("1234567890");   // Phone Number

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            // Act
            await _customerDataEntryService.CreateAsync();

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task CreateAsync_Should_Not_Create_Customer()
        {
            // Arrange
            _userInputServiceMock.SetupSequence(u => u.ReadLine())
                .Returns("John Doe")       // Name
                .Returns("john@example.com") // Email
                .Returns("e");   // Phone Number

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            // Act
            var res = await _customerDataEntryService.CreateAsync();

            // Assert
            Assert.Null(res);
        }

    }
}
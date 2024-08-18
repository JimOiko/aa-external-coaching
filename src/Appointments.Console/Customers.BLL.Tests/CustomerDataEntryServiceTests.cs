using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.DomainObjects.Interfaces;
using Customers.BLL.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net.Http.Json;

namespace Customers.BLL.Tests
{
    public class CustomerDataEntryServiceTests
    {
        private readonly Mock<IUserInputService> _userInputServiceMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly ICustomerDataEntryService _customerDataEntryService;
        private readonly Mock<IOptions<ApiSettings>> _apiSettingsMock;

        public CustomerDataEntryServiceTests()
        {
            _userInputServiceMock = new Mock<IUserInputService>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            // Mock IOptions<ApiSettings>
            _apiSettingsMock = new Mock<IOptions<ApiSettings>>();
            _apiSettingsMock.Setup(x => x.Value)
                            .Returns(new ApiSettings
                            {
                                NameDayApiBaseUrl = "",
                                DefaultCountry = "",
                                AppointmentApiUrl = "",
                                CustomerApiUrl = "https://localhost:7211/api/customers"
                            });

            // Instantiate the service with the mocked dependencies
            _customerDataEntryService = new CustomerDataEntryService(_httpClient, _apiSettingsMock.Object, _userInputServiceMock.Object);

            _customerDataEntryService = new CustomerDataEntryService(_httpClient, _apiSettingsMock.Object ,_userInputServiceMock.Object);
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

        [Fact]
        public async Task UpdateAsync_Should_Update_Customer()
        {
            // Arrange
            var existingCustomer = new Customer(name: "John Doe", email: "john.doe@example.com", phoneNumber: "1234567890", registrationDate: DateTimeOffset.Now);

            _userInputServiceMock.SetupSequence(u => u.ReadLine())
                .Returns("john.doe@example.com")  // Existing email
                .Returns("John Smith")            // New Name
                .Returns("john.smith@example.com") // New Email
                .Returns("0987654321");            // New Phone Number

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = JsonContent.Create(existingCustomer)
                });

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Put),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            // Act
            await _customerDataEntryService.UpdateAsync();

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri == new System.Uri($"https://localhost:7211/api/customers/update/{existingCustomer.Id}")),
                ItExpr.IsAny<CancellationToken>());

            _httpMessageHandlerMock.Protected().Verify(
               "SendAsync",
               Times.Once(),
               ItExpr.Is<HttpRequestMessage>(req =>
                   req.Method == HttpMethod.Get &&
                   req.RequestUri == new System.Uri($"https://localhost:7211/api/customers/getByEmail/{existingCustomer.Email}")),
               ItExpr.IsAny<CancellationToken>());
        }
    }
}
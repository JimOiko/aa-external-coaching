using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.DomainObjects.Interfaces;
using AppointmentManagementSystem.Services;
using Appointments.BLL.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net.Http.Json;
using System.Text.Json;

namespace Appointments.BLL.Tests
{
    public class AppointmentDataEntryServiceTests
    {
        private readonly Mock<IDiscountService> _discountServiceMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<IOptions<ApiSettings>> _apiSettingsMock;
        private readonly AppointmentDataEntryService _appointmentDataEntryService;
        private readonly Mock<IUserInputService> _userInputServiceMock;
        public AppointmentDataEntryServiceTests()
        {
            // Mock dependencies
            _userInputServiceMock = new Mock<IUserInputService>();
            _discountServiceMock = new Mock<IDiscountService>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            _apiSettingsMock = new Mock<IOptions<ApiSettings>>();
            _apiSettingsMock = new Mock<IOptions<ApiSettings>>();
            _apiSettingsMock.Setup(x => x.Value)
                            .Returns(new ApiSettings
                            {
                                NameDayApiBaseUrl = "https://nameday.abalin.net/api/V1/getdate",
                                DefaultCountry = "gr",
                                AppointmentApiUrl = "https://localhost:7188/api/appointments",
                                CustomerApiUrl = "https://localhost:7211/api/customers"
                            });

            _appointmentDataEntryService = new AppointmentDataEntryService(_httpClient, _discountServiceMock.Object, _apiSettingsMock.Object, _userInputServiceMock.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_MassageAppointment_With_UserInput()
        {
            // Arrange
            var customerEmail = "test@example.com";
            var customer = new Customer("Test User", customerEmail, "11111111", DateTimeOffset.Now);

            // Set up mock responses for user inputs
            _userInputServiceMock.SetupSequence(s => s.ReadLine())
                .Returns(customerEmail)                   // Email
                .Returns("1")                              // Service Type (Massage)
                .Returns("2024-08-20")                     // Date
                .Returns("15:00")                          // Time
                .Returns("Customer prefers a morning session.")  // Notes
                .Returns("1")                              // Massage Service (Relaxing Massage)
                .Returns("1");                             // Masseuse Preference (Male)

            // Set up mock HTTP responses
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath.Contains("getByEmail")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = JsonContent.Create(customer)
                });

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.AbsolutePath.Contains("create")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            // Act
            var response = await _appointmentDataEntryService.CreateAsync();

            // Assert
            _userInputServiceMock.Verify(s => s.ReadLine(), Times.Exactly(7));
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(2),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
            Assert.NotNull(response);
            Assert.True((int)((MassageAppointment)response).MassageServices == 0);
            Assert.True(response.Time == "15:00");
        }

        [Fact]
        public async Task CreateAsync_Customer_Not_Found_returns_Null()
        {
            // Arrange
            var customerEmail = "test@example.com";
            var customer = new Customer("Test User", customerEmail, "11111111", DateTimeOffset.Now);

            // Set up mock responses for user inputs
            _userInputServiceMock.SetupSequence(s => s.ReadLine())
                .Returns("test1@ecample1.com")                   // Email
                .Returns("1")                              // Service Type (Massage)
                .Returns("2024-08-20")                     // Date
                .Returns("15:00")                          // Time
                .Returns("Customer prefers a morning session.")  // Notes
                .Returns("1")                              // Massage Service (Relaxing Massage)
                .Returns("1");                             // Masseuse Preference (Male)

            // Set up mock HTTP responses
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath.Contains("getByEmail")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("{}", System.Text.Encoding.UTF8, "application/json") // Simulate empty JSON object
                });

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post && req.RequestUri.AbsolutePath.Contains("create")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            // Act
            var response = await _appointmentDataEntryService.CreateAsync();

            // Assert
            _userInputServiceMock.Verify(s => s.ReadLine(), Times.Exactly(1));
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
            Assert.Null(response);
        }

        [Fact]
        public async Task CreateAsync_Should_Handle_Invalid_ServiceType_Input()
        {
            // Arrange
            var customerEmail = "test@example.com";
            var customer = new Customer("Test User", customerEmail, "11111111", DateTimeOffset.Now);

            _userInputServiceMock.SetupSequence(s => s.ReadLine())
                .Returns(customerEmail)                   // Email
                .Returns("3");                             // Invalid Service Type

            // Set up mock HTTP responses
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath.Contains("getByEmail")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = JsonContent.Create(customer)
                });

            // Act
            await _appointmentDataEntryService.CreateAsync();

            // Assert
            _userInputServiceMock.Verify(s => s.ReadLine(), Times.Exactly(2));
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task ReadAsync_Should_Display_NoAppointments_When_EmptyListReturned()
        {
            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath.Contains("get")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("[]")  // Empty list of appointments
                });

            // Act
            await _appointmentDataEntryService.ReadAsync();

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),  // Verify that SendAsync was called exactly once
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath.Contains("get")),
                ItExpr.IsAny<CancellationToken>());

            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),  // Verify that SendAsync was called exactly once
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath.Contains("getById")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task ReadAsync_Should_Display_Appointments_When_NonEmptyListReturned()
        {
            // Arrange
            var appointments = new List<Appointment>
            {
                new MassageAppointment(
                    customerId: Guid.NewGuid(),
                    serviceType: AppointmentManagementSystem.DomainObjects.Enums.ServiceType.Massage,
                    date: DateTimeOffset.Now,
                    time: "10:00",
                    notes: "Relaxing massage",
                    massageServices: AppointmentManagementSystem.DomainObjects.Enums.MassageServices.RelaxingMassage,
                    preference: AppointmentManagementSystem.DomainObjects.Enums.MasseusePreference.Male
                ),
                new PersonalTrainingAppointment(
                    customerId: Guid.NewGuid(),
                    serviceType: AppointmentManagementSystem.DomainObjects.Enums.ServiceType.PersonalTraining,
                    date: DateTimeOffset.Now,
                    time: "11:00",
                    notes: "Training session",
                    trainingDuration: AppointmentManagementSystem.DomainObjects.Enums.TrainingDuration.OneHour,
                    customerComments: "Focus on legs",
                    injuriesOrPains: "Knee pain"
                )
            };

            var customers = new List<Customer>
            {
                new Customer ("John Doe","","", DateTimeOffset.Now ),
                new Customer ("Jane Smith", "", "", DateTimeOffset.Now)
            };
            customers[0].Id = appointments[0].CustomerId;
            customers[1].Id = appointments[1].CustomerId;

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath.Contains("get")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(appointments))
                });

            foreach (var customer in customers)
            {
                _httpMessageHandlerMock.Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.AbsolutePath.Contains($"getById/{customer.Id}")),
                        ItExpr.IsAny<CancellationToken>())
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = System.Net.HttpStatusCode.OK,
                        Content = JsonContent.Create(customer)
                    });
            }

            // Act
            await _appointmentDataEntryService.ReadAsync();

            // Assert
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Exactly(customers.Count + 1),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteAsync_Should_Delete_Appointment_When_ValidIdProvided()
        {
            // Arrange
            var appointmentId = Guid.NewGuid();
            _userInputServiceMock.Setup(s => s.ReadLine()).Returns(appointmentId.ToString());

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri.AbsolutePath.Contains($"delete/{appointmentId}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            // Act
            await _appointmentDataEntryService.DeleteAsync();

            // Assert
            _userInputServiceMock.Verify(s => s.ReadLine(), Times.Once);
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Delete && req.RequestUri.AbsolutePath.Contains($"delete/{appointmentId}")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteAsync_Should_Handle_InvalidIdInput()
        {
            // Arrange
            _userInputServiceMock.Setup(s => s.ReadLine()).Returns("InvalidGuid");

            // Act
            await _appointmentDataEntryService.DeleteAsync();

            // Assert
            _userInputServiceMock.Verify(s => s.ReadLine(), Times.Once);
            _httpMessageHandlerMock.Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>());
        }
    }
}
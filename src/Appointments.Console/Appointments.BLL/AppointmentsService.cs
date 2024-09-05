using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Appointments.BLL
{
    public class AppointmentsService : IAppointmentsService
    {
        private readonly IAppointmentRepository _appointmentRepo;
        private readonly IDiscountService _discountService;
        private readonly ILogger<AppointmentsService> _logger;
        public AppointmentsService(IAppointmentRepository appointmentRepo, IDiscountService discountService, ILogger<AppointmentsService> logger)
        {
            _appointmentRepo = appointmentRepo;
            _discountService = discountService;
            _logger = logger;
        }

        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepo.GetAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId)
        {
            return await _appointmentRepo.GetByIdAsync(appointmentId.ToString());
        }

        public async Task<bool> CreateAppointmentAsync(JsonElement appointmentElement)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new JsonStringEnumConverter(), new AppointmentJsonConverter() } // Add the custom converter
                };

                // Deserialize the appointment using the custom converter
                var appointment = JsonSerializer.Deserialize<Appointment>(appointmentElement.GetRawText(), options);

                if (appointment == null)
                {
                    throw new InvalidOperationException("Invalid appointment data.");
                }

                // Add the appointment to the repository (DB)
                await _appointmentRepo.AddAsync(appointment);

                // Call the discount service to check if it's the customer's nameday
                var isNameday = await _discountService.ProcessDiscountAsync(appointment.CustomerId, appointment.Date);
                return isNameday;
            }
            catch (InvalidOperationException ex)
            {
                // Handle specific InvalidOperationException
                _logger.LogError(ex, $"Operation failed: {ex.Message}");
                throw;
            }
            catch (HttpRequestException ex)
            {
                // Handle specific HttpRequestException from nameday API
                _logger.LogError(ex, $"Error with the nameday API: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                // Catch all other exceptions
                _logger.LogError(ex, $"An error occurred: {ex.Message}");
                throw;
            }
        }

        public async Task UpdateAppointmentAsync(JsonElement appointmentElement)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Converters = { new AppointmentJsonConverter() }
                };

                // Deserialize the JsonElement into an Appointment object
                var appointment = JsonSerializer.Deserialize<Appointment>(appointmentElement.GetRawText(), options);

                if (appointment == null)
                {
                    throw new ArgumentException("Failed to deserialize the appointment.");
                }

                // Now, proceed with updating the appointment in the repository.
                await _appointmentRepo.UpdateAsync(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the appointment.");
                throw;
            }
        }

        public async Task DeleteAppointmentAsync(Guid appointmentId)
        {
            var appointment = await _appointmentRepo.GetByIdAsync(appointmentId.ToString());
            if (appointment != null)
            {
                await _appointmentRepo.DeleteAsync(appointment);
            }
        }
    }
}

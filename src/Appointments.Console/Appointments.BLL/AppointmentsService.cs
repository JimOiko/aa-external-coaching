using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Extensions.Logging;

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

        public async Task<bool> CreateAppointmentAsync(Appointment appointment)
        {
            await _appointmentRepo.AddAsync(appointment);
            try
            {
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

        public async Task UpdateAppointmentAsync(Appointment appointment)
        {
            await _appointmentRepo.UpdateAsync(appointment);
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

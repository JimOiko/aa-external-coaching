using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Appointments.API
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly ILogger<AppointmentsController> _logger;
        private readonly IAppointmentsService _appointmentsService;

        public AppointmentsController(ILogger<AppointmentsController> logger, IAppointmentsService appointmentsService)
        {
            _logger = logger;
            _appointmentsService = appointmentsService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAsync()
        {
            try
            {
                var appointments = await _appointmentsService.GetAllAppointmentsAsync();

                if (appointments == null || !appointments.Any())
                {
                    _logger.LogInformation("No appointments found.");
                    return NotFound("No appointments found.");
                }

                return Ok(appointments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving appointments.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> GetByIdAsync(Guid id)
        {
            try
            {
                var appointment = await _appointmentsService.GetAppointmentByIdAsync(id);

                if (appointment == null)
                {
                    _logger.LogInformation($"Appointment with ID {id} not found.");
                    return NotFound($"Appointment with ID {id} not found.");
                }

                return Ok(appointment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving the appointment with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CreateAsync(JsonElement appointmentElement)
        {
            try
            {
                var isNameday = await _appointmentsService.CreateAppointmentAsync(appointmentElement);
                _logger.LogInformation("Appointment created successfully.");
                return Ok(new { IsNameday = isNameday });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the appointment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, JsonElement appointmentElement)
        {
            try
            {
                var existingAppointment = await _appointmentsService.GetAppointmentByIdAsync(id);
                if (existingAppointment == null)
                {
                    _logger.LogInformation($"Appointment with ID {id} not found.");
                    return NotFound($"Appointment with ID {id} not found.");
                }

                await _appointmentsService.UpdateAppointmentAsync(appointmentElement);
                _logger.LogInformation("Appointment updated successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the appointment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            try
            {
                var existingAppointment = await _appointmentsService.GetAppointmentByIdAsync(id);
                if (existingAppointment == null)
                {
                    _logger.LogInformation($"Appointment with ID {id} not found.");
                    return NotFound($"Appointment with ID {id} not found.");
                }

                await _appointmentsService.DeleteAppointmentAsync(id);
                _logger.LogInformation("Appointment deleted successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting the appointment with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

    }
}

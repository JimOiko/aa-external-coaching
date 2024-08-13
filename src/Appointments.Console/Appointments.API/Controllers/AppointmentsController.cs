using AppointmentManagementSystem.DomainObjects;
using Appointments.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.API.Controllers
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

        [HttpGet("get", Name = "GetAppointments")]
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

        [HttpGet("getByID/{id}", Name = "GetAppointmentById")]
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

        [HttpPost("create", Name = "CreateAppointment")]
        public async Task<ActionResult> CreateAsync(Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest("Appointment data is null.");
            }

            try
            {
                await _appointmentsService.CreateAppointmentAsync(appointment);
                _logger.LogInformation("Appointment created successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the appointment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("update", Name = "UpdateAppointment")]
        public async Task<ActionResult> UpdateAsync(Appointment appointment)
        {
            if (appointment == null)
            {
                return BadRequest("Appointment data is null.");
            }

            try
            {
                var existingAppointment = await _appointmentsService.GetAppointmentByIdAsync(appointment.AppointmentId);
                if (existingAppointment == null)
                {
                    _logger.LogInformation($"Appointment with ID {appointment.AppointmentId} not found.");
                    return NotFound($"Appointment with ID {appointment.AppointmentId} not found.");
                }

                await _appointmentsService.UpdateAppointmentAsync(appointment);
                _logger.LogInformation("Appointment updated successfully.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the appointment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("delete/{id}", Name = "DeleteAppointment")]
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

using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Text.Json;

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
        public async Task<ActionResult<bool>> CreateAsync(JsonElement appointmentElement)
        {
            try
            {
                Appointment appointment;
                var settings = new JsonSerializerSettings
                {
                    Converters = new List<Newtonsoft.Json.JsonConverter> { new StringEnumConverter() }
                };
                var baseAppointment = JsonConvert.DeserializeObject<Appointment>(appointmentElement.GetRawText(), settings);
                switch (baseAppointment?.ServiceType)
                {
                    case AppointmentManagementSystem.DomainObjects.Enums.ServiceType.PersonalTraining:
                        appointment = JsonConvert.DeserializeObject<PersonalTrainingAppointment>(appointmentElement.GetRawText(), settings);
                        break;

                    case AppointmentManagementSystem.DomainObjects.Enums.ServiceType.Massage:
                        appointment = JsonConvert.DeserializeObject<MassageAppointment>(appointmentElement.GetRawText(), settings);
                        break;

                    default:
                        return BadRequest("Unknown appointment type.");
                }
                var isNameday = await _appointmentsService.CreateAppointmentAsync(appointment);
                _logger.LogInformation("Appointment created successfully.");
                return Ok(new { IsNameday = isNameday });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the appointment.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("update/{appointmentId}", Name = "UpdateAppointment")]
        public async Task<ActionResult> UpdateAsync(Guid appointmentId, JsonElement appointmentElement)
        {
            try
            {
                var existingAppointment = await _appointmentsService.GetAppointmentByIdAsync(appointmentId);
                if (existingAppointment == null)
                {
                    _logger.LogInformation($"Appointment with ID {appointmentId} not found.");
                    return NotFound($"Appointment with ID {appointmentId} not found.");
                }

                Appointment appointment;
                var settings = new JsonSerializerSettings
                {
                    Converters = new List<Newtonsoft.Json.JsonConverter> { new StringEnumConverter() }
                };

                switch (existingAppointment.ServiceType)
                {
                    case AppointmentManagementSystem.DomainObjects.Enums.ServiceType.PersonalTraining:
                        appointment = JsonConvert.DeserializeObject<PersonalTrainingAppointment>(appointmentElement.GetRawText(),settings);
                        break;

                    case AppointmentManagementSystem.DomainObjects.Enums.ServiceType.Massage:
                        appointment = JsonConvert.DeserializeObject<MassageAppointment>(appointmentElement.GetRawText(), settings);
                        break;

                    default:
                        return BadRequest("Unknown appointment type.");
                }

                if (appointment == null)
                {
                    return BadRequest("Failed to deserialize the appointment.");
                }

                // Ensure that the correct appointment ID is used.
                appointment.AppointmentId = appointmentId;

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

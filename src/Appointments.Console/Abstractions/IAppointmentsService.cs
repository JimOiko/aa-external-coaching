using AppointmentManagementSystem.DomainObjects;
using System.Text.Json;

namespace AppointmentManagementSystem.Abstractions
{
    public interface IAppointmentsService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId);
        Task<bool> CreateAppointmentAsync(JsonElement appointment);
        Task UpdateAppointmentAsync(JsonElement appointment);
        Task DeleteAppointmentAsync(Guid appointmentId);
    }
}

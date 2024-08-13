using AppointmentManagementSystem.DomainObjects;

namespace Appointments.API.Interfaces
{
    public interface IAppointmentsService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId);
        Task CreateAppointmentAsync(Appointment appointment);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(Guid appointmentId);
    }
}

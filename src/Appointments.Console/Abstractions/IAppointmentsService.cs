using AppointmentManagementSystem.DomainObjects;

namespace AppointmentManagementSystem.Abstractions
{
    public interface IAppointmentsService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId);
        Task<bool> CreateAppointmentAsync(Appointment appointment);
        Task UpdateAppointmentAsync(Appointment appointment);
        Task DeleteAppointmentAsync(Guid appointmentId);
    }
}

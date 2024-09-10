
using AppointmentManagementSystem.DomainObjects;

namespace AppointmentManagementSystem.Abstractions
{
    public interface IAppointmentDataEntryService
    {
        Task<Appointment?> CreateAsync();
        Task ReadAsync();
        Task UpdateAsync();
        Task DeleteAsync();
    }
}

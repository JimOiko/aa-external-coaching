
using AppointmentManagementSystem.DomainObjects;

namespace Appointments.BLL.Interfaces
{
    public interface IAppointmentDataEntryService
    {
        Task<Appointment?> CreateAsync();
        Task ReadAsync();
        Task UpdateAsync();
        Task DeleteAsync();
    }
}

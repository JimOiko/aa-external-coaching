
namespace Appointments.BLL.Interfaces
{
    public interface IAppointmentDataEntryService
    {
        Task CreateAsync();
        Task ReadAsync();
        Task UpdateAsync();
        Task DeleteAsync();
    }
}

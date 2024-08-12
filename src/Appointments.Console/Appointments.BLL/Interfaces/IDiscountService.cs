
namespace Appointments.BLL.Interfaces
{
    public interface IDiscountService
    {
        Task<bool> ProcessDiscountAsync(Guid customerId, DateTimeOffset appointmentDate);
    }
}

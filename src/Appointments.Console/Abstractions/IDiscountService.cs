
namespace AppointmentManagementSystem.Abstractions
{
    public interface IDiscountService
    {
        Task<bool> ProcessDiscountAsync(Guid customerId, DateTimeOffset appointmentDate);
    }
}

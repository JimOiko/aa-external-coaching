using AppointmentManagementSystem.DomainObjects;

namespace Customers.API.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(Guid customerId);
        Task<Customer?> GetCustomerByEmailAsync(string email);
        Task CreateCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(string email);
    }
}

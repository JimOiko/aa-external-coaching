using AppointmentManagementSystem.DomainObjects;

namespace Customers.DAL.Interfaces
{
    public interface ICustomerRepository
    {
        Task AddAsync(Customer item);
        Task<List<Customer>> GetAsync();
        Task<Customer?> GetByIdAsync(Guid id);
        Task<Customer?> GetByEmailAsync(string email);
        Task UpdateAsync(Customer item);
        Task DeleteAsync(Customer item);
        Task<int> GetCountAsync();
        Task<List<Customer>> GetNewCustomersByDateAsync(DateTimeOffset date);
    }
}

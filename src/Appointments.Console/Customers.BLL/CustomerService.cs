using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Abstractions;

namespace Customers.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAsync();
            return customers;
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            return customer;
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            return customer;
        }

        public async Task CreateCustomerAsync(Customer customerDto)
        {
            var customer = customerDto;
            await _customerRepository.AddAsync(customer);
        }

        public async Task UpdateCustomerAsync(Customer customerDto)
        {
            await _customerRepository.UpdateAsync(customerDto);
        }

        public async Task DeleteCustomerAsync(string email)
        {
            var customer = await _customerRepository.GetByEmailAsync(email);
            await _customerRepository.DeleteAsync(customer);
        }
    }
}

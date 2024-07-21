using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Infastructure.Interfaces;

namespace AppointmentManagementSystem.Infastructure
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers = [];

        #region CRUD
        public void Add(Customer customer)
        {
            _customers.Add(customer);
        }

        public List<Customer> Get()
        {
            return (List<Customer>)_customers.Clone();
        }

        public Customer? GetById(string id)
        {
            var existingCustomer = _customers.FirstOrDefault(c => c.Email.Equals(id, StringComparison.OrdinalIgnoreCase));
            return existingCustomer;
        }

        public void Delete(Customer customer)
        {
            _customers.Remove(customer);
        }
        #endregion CRUD

        #region Reporting
        public int GetCount()
        {
            return _customers.Count;
        }

        public List<Customer> GetNewCustomersByDate(DateTime date)
        {
            return _customers.Where(c => c.RegistrationDate.Date == date.Date).Select(c => (Customer)c.Clone()).ToList();
        }
    }
    #endregion Reporting
}

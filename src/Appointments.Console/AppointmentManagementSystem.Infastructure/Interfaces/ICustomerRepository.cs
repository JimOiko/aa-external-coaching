using AppointmentManagementSystem.DomainObjects;

namespace AppointmentManagementSystem.Infastructure.Interfaces
{
    public interface ICustomerRepository
    {
        void Add(Customer item);
        List<Customer> Get();
        Customer? GetById(string id);
        void Update(Customer item);
        void Delete(Customer item);
        int GetCount();
        List<Customer> GetNewCustomersByDate(DateTime date);
    }
}

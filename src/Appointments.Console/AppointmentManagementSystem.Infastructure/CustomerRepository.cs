using AppManagementSystem.DbObjects;
using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Infastructure.Interfaces;

namespace AppointmentManagementSystem.Infastructure
{
    public class CustomerRepository(AppointmentManagementContext db) : ICustomerRepository
    {
        #region CRUD
        public void Add(Customer customer)
        {
            db.Customer.Add(customer);
            db.SaveChanges();
        }

        public List<Customer> Get()
        {
            return [.. db.Customer];
        }

        public Customer? GetById(int id) => db.Customer.FirstOrDefault(c => c.Id == id);

        public Customer? GetByEmail(string email)
        {
            return db.Customer.FirstOrDefault(c => c.Email.ToLower() == email.ToLower()); ;
        }

        public void Update(Customer customer)
        {
            db.Customer.Update(customer);
            db.SaveChanges();
        }

        public void Delete(Customer customer)
        {
            db.Customer.Remove(customer);
            db.SaveChanges();
        }
        #endregion CRUD

        #region Reporting
        public int GetCount()
        {
            return db.Customer.Count();
        }

        public List<Customer> GetNewCustomersByDate(DateTimeOffset date)
        {
            return [.. db.Customer.Where(c => c.RegistrationDate.Date == date.Date).Select(c => c)];
        }
    }
    #endregion Reporting
}

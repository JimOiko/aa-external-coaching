using AppManagementSystem.DbObjects;
using AppointmentManagementSystem.DomainObjects;
using AppointmentManagementSystem.Infastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppointmentManagementSystem.Infastructure
{
    public class CustomerRepository(AppointmentManagementContext db) : ICustomerRepository
    {
        #region CRUD
        public async Task AddAsync(Customer customer)
        {
            await db.Customer.AddAsync(customer);
            await db.SaveChangesAsync();
        }

        public async Task<List<Customer>> GetAsync()
        {
            return await db.Customer.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(Guid id)
        {
            return await db.Customer.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer?> GetByEmailAsync(string email)
        {
            return await db.Customer.FirstOrDefaultAsync(c => c.Email.ToLower() == email.ToLower());
        }

        public async Task UpdateAsync(Customer customer)
        {
            db.Customer.Update(customer);
            await db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Customer customer)
        {
            db.Customer.Remove(customer);
            await db.SaveChangesAsync();
        }
        #endregion CRUD

        #region Reporting
        public async Task<int> GetCountAsync()
        {
            return await db.Customer.CountAsync();
        }

        public async Task<List<Customer>> GetNewCustomersByDateAsync(DateTimeOffset date)
        {
            return await db.Customer.Where(c => c.RegistrationDate.Date == date.Date).ToListAsync();
        }
        #endregion Reporting
    }
}

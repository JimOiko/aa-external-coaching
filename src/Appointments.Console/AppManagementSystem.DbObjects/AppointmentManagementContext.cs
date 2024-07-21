using Microsoft.EntityFrameworkCore;
using AppointmentManagementSystem.DomainObjects;

namespace AppManagementSystem.DbObjects
{
    public class AppointmentManagementContext: DbContext
    {
        public DbSet<Customer> Customer { get; }
    }
}

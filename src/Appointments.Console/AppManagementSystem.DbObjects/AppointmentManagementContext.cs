using Microsoft.EntityFrameworkCore;
using AppointmentManagementSystem.DomainObjects;

namespace AppManagementSystem.DbObjects
{
    public class AppointmentManagementContext: DbContext
    {
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<MassageAppointment> MassageAppointment { get; set; }
        public DbSet<PersonalTrainingAppointment> PersonalTrainingAppointment { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Data Source = DESKTOP-KPO7S6Q;TrustServerCertificate=True;Initial Catalog=AppointmentManagement; Integrated Security = True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using AppointmentManagementSystem.DomainObjects;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace AppointmentManagementSystem.DbObjects
{
    public class AppointmentManagementContext : DbContext
    {
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<MassageAppointment> MassageAppointment { get; set; }
        public DbSet<PersonalTrainingAppointment> PersonalTrainingAppointment { get; set; }
        private readonly IConfiguration _configuration;

        public AppointmentManagementContext(DbContextOptions<AppointmentManagementContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var parameters = new object[] { modelBuilder };
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var method = entityType.ClrType.GetMethod("OnModelCreating", BindingFlags.Static | BindingFlags.NonPublic);
                if (method != null)
                    method.Invoke(null, parameters);
            }

        }
    }
}

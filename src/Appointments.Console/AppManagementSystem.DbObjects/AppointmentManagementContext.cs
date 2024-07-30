using Microsoft.EntityFrameworkCore;
using AppointmentManagementSystem.DomainObjects;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace AppManagementSystem.DbObjects
{
    public class AppointmentManagementContext : DbContext
    {
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<MassageAppointment> MassageAppointment { get; set; }
        public DbSet<PersonalTrainingAppointment> PersonalTrainingAppointment { get; set; }
        private readonly IConfiguration _configuration;

        public AppointmentManagementContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("AppointmentManagementDatabase");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>().UseTptMappingStrategy();

            modelBuilder
                .Entity<Appointment>()
                .Property(e => e.ServiceType)
                .HasConversion<int>();

            modelBuilder.Entity<Appointment>()
                .HasOne<Customer>()
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .IsRequired();

            // Seed data for ServiceType enum (Optional)
            modelBuilder.Entity<ServiceType>().HasData(
                Enum.GetValues(typeof(ServiceTypeEnum))
                    .Cast<ServiceTypeEnum>()
                    .Select(e => new ServiceType()
                    {
                        ServiceTypeId = e,
                        Name = e.ToString(),
                    })
            );

            modelBuilder.Entity<MasseusePreference>().HasKey(m => m.PreferenceId);
            modelBuilder.Entity<MasseusePreference>().HasData(
                Enum.GetValues(typeof(MasseusePreferenceEnum))
                    .Cast<MasseusePreferenceEnum>()
                    .Select(e => new MasseusePreference()
                    {
                        PreferenceId = e,
                        Name = e.ToString(),
                    })
            );
            modelBuilder.Entity<MassageServices>().HasKey(m => m.MassageServiceId);
            modelBuilder.Entity<MassageServices>().HasData(
                Enum.GetValues(typeof(MassageServicesEnum))
                    .Cast<MassageServicesEnum>()
                    .Select(e => new MassageServices()
                    {
                        MassageServiceId = e,
                        Name = e.ToString(),
                    })
            );

            modelBuilder.Entity<TrainingDuration>().HasData(
                Enum.GetValues(typeof(TrainingDurationEnum))
                    .Cast<TrainingDurationEnum>()
                    .Select(e => new TrainingDuration()
                    {
                        trainingDurationId = e,
                        Name = e.ToString(),
                    })
            );
        }
    }
}

using Microsoft.EntityFrameworkCore;
using AppointmentManagementSystem.DomainObjects;
using Microsoft.Extensions.Configuration;

namespace AppManagementSystem.DbObjects
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;

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
                Enum.GetValues(typeof(AllEnums.ServiceType))
                    .Cast<AllEnums.ServiceType>()
                    .Select(e => new ServiceType()
                    {
                        ServiceTypeId = e,
                        Name = e.ToString(),
                    })
            );

            modelBuilder.Entity<MasseusePreference>().HasKey(m => m.PreferenceId);
            modelBuilder.Entity<MasseusePreference>().HasData(
                Enum.GetValues(typeof(AllEnums.MasseusePreference))
                    .Cast<AllEnums.MasseusePreference>()
                    .Select(e => new MasseusePreference()
                    {
                        PreferenceId = e,
                        Name = e.ToString(),
                    })
            );
            modelBuilder.Entity<MassageServices>().HasKey(m => m.MassageServiceId);
            modelBuilder.Entity<MassageServices>().HasData(
                Enum.GetValues(typeof(AllEnums.MassageServices))
                    .Cast<AllEnums.MassageServices>()
                    .Select(e => new MassageServices()
                    {
                        MassageServiceId = e,
                        Name = e.ToString(),
                    })
            );

            modelBuilder.Entity<TrainingDuration>().HasData(
                Enum.GetValues(typeof(AllEnums.TrainingDuration))
                    .Cast<AllEnums.TrainingDuration>()
                    .Select(e => new TrainingDuration()
                    {
                        TrainingDurationId = e,
                        Name = e.ToString(),
                    })
            );
        }
    }
}


using Microsoft.EntityFrameworkCore;

namespace AppointmentManagementSystem.DomainObjects
{
    using Constants = AppointmentManagementSystem.DomainObjects.Enums;

    public class MasseusePreference
    {
        public required AppointmentManagementSystem.DomainObjects.Enums.MasseusePreference PreferenceId { get; set; }
        public required string Name { get; set; }

        protected static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MasseusePreference>().HasKey(m => m.PreferenceId);
            modelBuilder.Entity<MasseusePreference>().HasData(
                Enum.GetValues(typeof(Constants.MasseusePreference))
                    .Cast<Constants.MasseusePreference>()
                    .Select(e => new MasseusePreference()
                    {
                        PreferenceId = e,
                        Name = e.ToString(),
                    })
            );
        }
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagementSystem.DomainObjects
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;

    public class MasseusePreference
    {
        public required AppointmentManagementSystem.DomainObjects.Enums.MasseusePreference PreferenceId { get; set; }
        public required string Name { get; set; }

        protected static void OnModelCreating(ModelBuilder modelBuilder)
        {
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
        }
    }
}


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;

namespace AppointmentManagementSystem.DomainObjects
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;

    public class TrainingDuration
    {
        public required AppointmentManagementSystem.DomainObjects.Enums.TrainingDuration TrainingDurationId { get; set; }
        public required string Name { get; set; }

        protected static void OnModelCreating(ModelBuilder modelBuilder)
        {
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

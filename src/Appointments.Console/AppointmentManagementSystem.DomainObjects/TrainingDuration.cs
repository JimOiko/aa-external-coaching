using Microsoft.EntityFrameworkCore;

namespace AppointmentManagementSystem.DomainObjects
{
    using Constants = AppointmentManagementSystem.DomainObjects.Enums;

    public class TrainingDuration
    {
        public required AppointmentManagementSystem.DomainObjects.Enums.TrainingDuration TrainingDurationId { get; set; }
        public required string Name { get; set; }

        protected static void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TrainingDuration>().HasData(
                Enum.GetValues(typeof(Constants.TrainingDuration))
                    .Cast<Constants.TrainingDuration>()
                    .Select(e => new TrainingDuration()
                    {
                        TrainingDurationId = e,
                        Name = e.ToString(),
                    })
            );
        }
    }
}

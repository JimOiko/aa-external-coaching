
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagementSystem.DomainObjects
{
    public class TrainingDuration
    {
        public required AppointmentManagementSystem.DomainObjects.Enums.TrainingDuration TrainingDurationId { get; set; }
        public required string Name { get; set; }
    }
}

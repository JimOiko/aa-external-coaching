
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagementSystem.DomainObjects
{
    public class TrainingDuration
    {
        public required TrainingDurationEnum trainingDurationId { get; set; }
        public required string Name { get; set; }
    }
}

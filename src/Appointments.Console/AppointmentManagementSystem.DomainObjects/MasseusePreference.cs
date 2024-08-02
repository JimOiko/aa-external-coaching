
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagementSystem.DomainObjects
{
    public class MasseusePreference
    {
        public required AppointmentManagementSystem.DomainObjects.Enums.MasseusePreference PreferenceId { get; set; }
        public required string Name { get; set; }
    }
}

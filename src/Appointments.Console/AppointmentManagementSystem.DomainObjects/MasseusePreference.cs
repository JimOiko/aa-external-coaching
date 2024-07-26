
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagementSystem.DomainObjects
{
    public class MasseusePreference
    {
        public required MasseusePreferenceEnum PreferenceId { get; set; }
        public required string Name { get; set; }
    }
}

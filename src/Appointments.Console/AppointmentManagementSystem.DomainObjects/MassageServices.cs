
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagementSystem.DomainObjects
{
    public class MassageServices
    {
        public required AppointmentManagementSystem.DomainObjects.Enums.MassageServices MassageServiceId { get; set; }
        public required string Name { get; set; }
    }
}

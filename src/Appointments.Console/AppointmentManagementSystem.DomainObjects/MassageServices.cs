
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagementSystem.DomainObjects
{
    public class MassageServices
    {
        public required MassageServicesEnum MassageServiceId { get; set; }
        public required string Name { get; set; }
    }
}

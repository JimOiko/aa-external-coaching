
using System.ComponentModel.DataAnnotations;

namespace AppointmentManagementSystem.DomainObjects
{
    public class ServiceType
    {
        public required ServiceTypeEnum ServiceTypeId{ get; set; }
        public required string Name { get; set; }
    }
}

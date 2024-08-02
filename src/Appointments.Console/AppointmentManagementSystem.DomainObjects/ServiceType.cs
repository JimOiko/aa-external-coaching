
namespace AppointmentManagementSystem.DomainObjects
{
    public class ServiceType
    {
        
        public required AppointmentManagementSystem.DomainObjects.Enums.ServiceType ServiceTypeId { get; set; }
        public required string Name { get; set; }
    }
}

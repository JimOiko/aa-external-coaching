namespace AppointmentManagementSystem.DomainObjects
{
    public class ServiceTypeMaxAppointments
    {
        public AppointmentManagementSystem.DomainObjects.Enums.ServiceType ServiceType { get; set; }
        public DateTimeOffset? Date { get; set; }
        public int Count { get; set; }
    }
}
namespace AppointmentManagementSystem.DomainObjects
{
    public class ServiceTypeMaxAppointments
    {
        public ServiceType ServiceType { get; set; }
        public DateTimeOffset? Date { get; set; }
        public int Count { get; set; }
    }
}
namespace AppointmentManagementSystem.DomainObjects
{
    public class ServiceTypeMaxAppointments
    {
        public ServiceTypeEnum ServiceType { get; set; }
        public DateTimeOffset? Date { get; set; }
        public int Count { get; set; }
    }
}
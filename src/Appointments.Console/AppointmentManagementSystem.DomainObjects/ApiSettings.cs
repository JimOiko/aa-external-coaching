namespace AppointmentManagementSystem.DomainObjects
{
    public class ApiSettings
    {
        public required string NameDayApiBaseUrl { get; set; }
        public required string DefaultCountry { get; set; }
        public required string CustomerApiUrl { get; set; }
        public required string AppointmentApiUrl { get; set; }
    }
}

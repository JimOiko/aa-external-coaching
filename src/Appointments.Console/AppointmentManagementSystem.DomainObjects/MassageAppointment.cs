
namespace AppointmentManagementSystem.DomainObjects
{
    using System.Text.Json.Serialization;
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public class MassageAppointment: Appointment
    {
        private MassageAppointment() { }

        public MassageAppointment(Guid customerId, AllEnums.ServiceType serviceType, DateTimeOffset date, string? time, string? notes,
        AllEnums.MassageServices massageServices, AllEnums.MasseusePreference preference) : base(customerId,
                                                                               serviceType,
                                                                               date,
                                                                               time,
                                                                               notes)
        {
            MassageServices = massageServices;
            Preference = preference;
        }
        
        [JsonPropertyName("massageServices")]
        public AllEnums.MassageServices MassageServices { get; set; }
        
        [JsonPropertyName("preference")]
        public AllEnums.MasseusePreference Preference { get; set; }

        public override string ToString()
        {
            return base.ToString() + $", Service Type: {ServiceType}, Masseuse Preference: {Preference}";
        }
    }
}

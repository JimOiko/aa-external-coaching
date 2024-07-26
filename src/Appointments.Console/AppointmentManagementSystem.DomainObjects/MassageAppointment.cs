using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppointmentManagementSystem.DomainObjects
{
    public class MassageAppointment: Appointment
    {

        private MassageAppointment() { }

        public MassageAppointment(int customerId, ServiceTypeEnum serviceType, DateTimeOffset date, string? time, string? notes,
        MassageServicesEnum massageServices, MasseusePreferenceEnum preference) : base(customerId,
                                                                               serviceType,
                                                                               date,
                                                                               time,
                                                                               notes)
        {
            MassageServices = massageServices;
            Preference = preference;
        }

        public MassageServicesEnum MassageServices { get; set; }
        public MasseusePreferenceEnum Preference { get; set; }

        public override string ToString()
        {
            return base.ToString() + $", Service Type: {ServiceType}, Masseuse Preference: {Preference}";
        }
    }
}

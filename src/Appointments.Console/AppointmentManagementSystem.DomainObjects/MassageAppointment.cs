namespace AppointmentManagementSystem.DomainObjects
{
    using Constants = AppointmentManagementSystem.DomainObjects.Enums;
    public class MassageAppointment: Appointment
    {
        private MassageAppointment() { }

        public MassageAppointment(Guid customerId, Constants.ServiceType serviceType, DateTimeOffset date, string? time, string? notes,
        Constants.MassageServices massageServices, Constants.MasseusePreference preference) : base(customerId,
                                                                               serviceType,
                                                                               date,
                                                                               time,
                                                                               notes)
        {
            MassageServices = massageServices;
            Preference = preference;
        }
        
        public Constants.MassageServices MassageServices { get; set; }
        public Constants.MasseusePreference Preference { get; set; }

        public override string ToString()
        {
            return base.ToString() + $", Service Type: {ServiceType}, Masseuse Preference: {Preference}";
        }
    }
}

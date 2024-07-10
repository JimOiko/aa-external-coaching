namespace AppointmentManagementSystem.Models
{
    public class MassageAppointment : Appointment
    {
        public MassageServices MassageServices { get; set; }
        public MasseusePreference Preference { get; set; }
        public MassageAppointment(Customer customer, ServiceType serviceType, DateTime date, string time, string notes,
            MassageServices massageServices, MasseusePreference preference) : base(customer, serviceType, date, time, notes)
        {
            MassageServices = massageServices;
            Preference = preference;
        }

        public override string ToString()
        {
            return base.ToString() + $", Service Type: {ServiceType}, Masseuse Preference: {Preference}";
        }
    }
}

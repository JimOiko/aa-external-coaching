namespace AppointmentManagementSystem.Models
{
    public class MassageAppointment(Customer customer, ServiceType serviceType, DateTime date, string time, string notes,
        MassageServices massageServices, MasseusePreference preference) : Appointment(customer, serviceType, date, time, notes)
    {
        public MassageServices MassageServices { get; set; } = massageServices;
        public MasseusePreference Preference { get; set; } = preference;

        public override string ToString()
        {
            return base.ToString() + $", Service Type: {ServiceType}, Masseuse Preference: {Preference}";
        }
    }
}

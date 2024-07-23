using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AppointmentManagementSystem.DomainObjects
{
    public class MassageAppointment: Appointment
    {

        public MassageAppointment() { }

        public MassageAppointment(Customer customer, ServiceType serviceType, DateTime date, string time, string notes,
        MassageServices massageServices, MasseusePreference preference) : base(customer,
                                                                               serviceType,
                                                                               date,
                                                                               time,
                                                                               notes) { }

        public MassageServices MassageServices { get; set; }
        public MasseusePreference Preference { get; set; }

        public override string ToString()
        {
            return base.ToString() + $", Service Type: {ServiceType}, Masseuse Preference: {Preference}";
        }
    }
}

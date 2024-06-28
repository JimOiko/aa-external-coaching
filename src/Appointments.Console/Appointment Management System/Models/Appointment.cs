using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Management_System.Models
{
    public class Appointment
    {
        private static int NextId = 1; //so as not to use guid
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public ServiceType ServiceType { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public string Notes { get; set; }

        public Appointment(Customer customer, ServiceType serviceType, DateTime date, string time, string notes)
        {
            Id = Interlocked.Increment(ref NextId);
            Customer = customer;
            ServiceType = serviceType;
            Date = date;
            Time = time;
            Notes = notes;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Customer: {Customer.Name}, Service: {ServiceType}, Date: {Date.ToShortDateString()}, Time: {Time}, Notes: {Notes}";
        }
    }

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

    public class PersonalTrainingAppointment : Appointment
    {
        public TrainingDuration TrainingDuration { get; set; }
        public string CustomerComments { get; set; }
        public string InjuriesOrPains { get; set; }
        public PersonalTrainingAppointment(Customer customer, ServiceType serviceType, DateTime date, string time, string notes,
                                       TrainingDuration trainingDuration, string customerComments, string injuriesOrPains)
        : base(customer, serviceType, date, time, notes)
        {
            TrainingDuration = trainingDuration;
            CustomerComments = customerComments;
            InjuriesOrPains = injuriesOrPains;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Customer: {Customer.Name}, Service: {ServiceType}, Date: {Date.ToShortDateString()}, Time: {Time}, " +
                   $"Notes: {Notes}, Duration: {GetTrainingDurationDescription()}, Comments: {CustomerComments}, Injuries/Pains: {InjuriesOrPains}";
        }

        private string GetTrainingDurationDescription()
        {
            switch (TrainingDuration)
            {
                case TrainingDuration.ThirtyMinutes:
                    return "30 minutes";
                case TrainingDuration.OneHour:
                    return "1 hour";
                case TrainingDuration.OneHourThirtyMinutes:
                    return "1 hour and 30 minutes";
                default:
                    return "Unknown";
            }
        }
    }
}

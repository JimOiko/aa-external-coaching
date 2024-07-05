namespace Appointment_Management_System.Models
{
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

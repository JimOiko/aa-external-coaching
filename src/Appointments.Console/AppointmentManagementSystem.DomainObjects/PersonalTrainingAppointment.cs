namespace AppointmentManagementSystem.DomainObjects
{
    public class PersonalTrainingAppointment : Appointment
    {
        private PersonalTrainingAppointment() { }

        public PersonalTrainingAppointment(int customerId, ServiceTypeEnum serviceType, DateTimeOffset date, string? time, string? notes,
                                       TrainingDurationEnum? trainingDuration, string? customerComments, string? injuriesOrPains)
        : base(customerId, serviceType, date, time, notes)
        {
            TrainingDuration = trainingDuration;
            CustomerComments = customerComments;
            InjuriesOrPains = injuriesOrPains;
        }

        public TrainingDurationEnum? TrainingDuration { get; set; }
        public string? CustomerComments { get; set; }
        public string? InjuriesOrPains { get; set; }
        

        public override string ToString()
        {
            return $"ID: {AppointmentId}, Service: {ServiceType}, Date: {Date.DateTime.ToShortDateString()}, Time: {Time}, " +
                   $"Notes: {Notes}, Duration: {GetTrainingDurationDescription()}, Comments: {CustomerComments}, Injuries/Pains: {InjuriesOrPains}";
        }

        private string GetTrainingDurationDescription()
        {
            switch (TrainingDuration)
            {
                case TrainingDurationEnum.ThirtyMinutes:
                    return "30 minutes";
                case TrainingDurationEnum.OneHour:
                    return "1 hour";
                case TrainingDurationEnum.OneHourThirtyMinutes:
                    return "1 hour and 30 minutes";
                default:
                    return "Unknown";
            }
        }
    }
}

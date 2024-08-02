namespace AppointmentManagementSystem.DomainObjects
{
    using AllEnums = AppointmentManagementSystem.DomainObjects.Enums;
    public class PersonalTrainingAppointment : Appointment
    {
        private PersonalTrainingAppointment() { }

        public PersonalTrainingAppointment(Guid customerId, AllEnums.ServiceType serviceType, DateTimeOffset date, string? time, string? notes,
                                       AllEnums.TrainingDuration? trainingDuration, string? customerComments, string? injuriesOrPains)
        : base(customerId, serviceType, date, time, notes)
        {
            TrainingDuration = trainingDuration;
            CustomerComments = customerComments;
            InjuriesOrPains = injuriesOrPains;
        }

        public AllEnums.TrainingDuration? TrainingDuration { get; set; }
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
                case AllEnums.TrainingDuration.ThirtyMinutes:
                    return "30 minutes";
                case AllEnums.TrainingDuration.OneHour:
                    return "1 hour";
                case AllEnums.TrainingDuration.OneHourThirtyMinutes:
                    return "1 hour and 30 minutes";
                default:
                    return "Unknown";
            }
        }
    }
}

namespace AppointmentManagementSystem.Abstractions
{
    public interface INameDayApiClient
    {
        Task<string> GetNamedayAsync(DateTimeOffset date);
    }
}

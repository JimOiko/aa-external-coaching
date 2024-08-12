namespace Appointments.DAL.Interfaces
{
    public interface INameDayApiClient
    {
        Task<string> GetNamedayAsync(DateTimeOffset date);
    }
}

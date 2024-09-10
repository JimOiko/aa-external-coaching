namespace AppointmentManagementSystem.Abstractions {
    public interface ICustomerReportService
    {
        Task GetRegisteredCustomerAsync();
        Task GetNewCustomersByDateAsync(DateTimeOffset date);

    }
}

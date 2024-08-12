namespace Customers.BLL.Interfaces
{
    public interface ICustomerReportService
    {
        Task GetRegisteredCustomerAsync();
        Task GetNewCustomersByDateAsync(DateTimeOffset date);

    }
}

namespace Customers.BLL.Interfaces
{
    public interface ICustomerDataEntryService
    {
        Task CreateAsync();
        Task ReadAsync();
        Task UpdateAsync();
        Task DeleteAsync();
    }
}

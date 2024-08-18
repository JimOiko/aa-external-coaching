namespace Customers.BLL.Interfaces
{
    public interface ICustomerDataEntryService
    {
        Task<object?> CreateAsync();
        Task ReadAsync();
        Task UpdateAsync();
        Task DeleteAsync();
    }
}

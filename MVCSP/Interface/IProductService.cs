using MVCSP.Models;

namespace MVCSP.Interface
{
    public interface IProductService
    {
        Task AddAsync(Product data);
        Task DeleteAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int Id);
        Task<(IEnumerable<Product> data, int totalCount)> GetPaginatedData(int page, int pagesize, string search);
        Task UpdateAsync(Product data);
    }
}
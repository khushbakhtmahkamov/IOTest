using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductService.Repositories.Product
{
    public interface IProductRepository : IBaseRepository<Models.Product>
    {
        Task<IEnumerable<Models.Product>> GetProductsAsync(int pageNumber, int pageSize);
        Task<int> GetCount();
    }
}

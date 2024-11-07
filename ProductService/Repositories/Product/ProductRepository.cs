using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Repositories.Product
{
    public class ProductRepository : BaseRepository<Models.Product>, IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetCount()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<IEnumerable<Models.Product>> GetProductsAsync(int pageNumber, int pageSize)
        {
            var products = await _context.Products
            .Include(p => p.Category)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

            return products;
        }

        public override async Task<Models.Product> GetByIdAsync(int id)
        {
            return await _context.Products
                             .Include(p => p.Category)
                             .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}

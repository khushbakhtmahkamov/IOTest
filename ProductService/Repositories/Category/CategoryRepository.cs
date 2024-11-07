using ProductService.Data;

namespace ProductService.Repositories.Category
{
    public class CategoryRepository : BaseRepository<Models.Category>, ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
  
    }
}

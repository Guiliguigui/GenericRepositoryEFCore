using Microsoft.EntityFrameworkCore;

namespace GenericRepositoryEFCore
{
    public class BaseRepository
    {
        protected readonly DbContext _db;
        public BaseRepository(DbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }
    }
}

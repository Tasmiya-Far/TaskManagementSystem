using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Core;
using TaskManagementSystem.Core.Dbcontext;


namespace TaskManagementSystem.Infrastructure.Repositories
{
    //Repository for Generic
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _context;

        protected GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> Get(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}

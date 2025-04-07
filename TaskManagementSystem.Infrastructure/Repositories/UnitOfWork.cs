using TaskManagementSystem.Core;
using TaskManagementSystem.Core.Dbcontext;
using TaskManagementSystem.Core.Intefaces;

namespace TaskManagementSystem.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public IUserRepository User { get; }
        public ITaskItemRepository TaskItem { get; }

        public UnitOfWork(AppDbContext DbContext, IUserRepository userRepository,
            ITaskItemRepository TaskItemRepository)
        {
            this._context = DbContext;
            this.User = userRepository;
            this.TaskItem = TaskItemRepository;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

    }
}

using TaskManagementSystem.Core.Intefaces;

namespace TaskManagementSystem.Core
{
    //interface for unit of work
    public interface IUnitOfWork : IDisposable
    {
        ITaskItemRepository TaskItem { get; }
        IUserRepository User { get; }

        int Complete();
    }
}

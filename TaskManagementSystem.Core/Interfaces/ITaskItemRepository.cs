using TaskManagementSystem.Core.Models;

namespace TaskManagementSystem.Core.Intefaces
{
    public interface ITaskItemRepository : IGenericRepository<TaskItem>
    {
        Task<TaskItem> GetTaskItemByID(string id);
        Task<List<TaskItem>> GetAllTaskItemListByUserID(string id);

    }
}

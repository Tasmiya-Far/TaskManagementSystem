using TaskManagementSystem.Core.Models;
using TaskManagementSystem.Core.ViewModel;

namespace TaskManagementSystem.Core.Intefaces
{
    public interface ITaskItemRepository : IGenericRepository<TaskItem>
    {
        Task<TaskItem> GetTaskItemByID(string id);
        Task<PagedResult<TaskItem>> GetAllTaskItemListByUserID(string userId, PageParams paginatedParams);

    }
}

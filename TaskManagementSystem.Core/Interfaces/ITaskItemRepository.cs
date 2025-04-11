using TaskManagementSystem.Core.Models;
using TaskManagementSystem.Core.ViewModel;

namespace TaskManagementSystem.Core.Intefaces
{
    public interface ITaskItemRepository : IGenericRepository<TaskItem>
    {
        Task<TaskItem> GetTaskItemByID(string id);
        //Task<List<TaskItem>> GetAllTaskItemListByUserID(string id);
        //Task<List<TaskItem>> GetAllTaskItemListByUserID(string id, TaskQueryParamsViewModel query);
        Task<PagedResult<TaskItem>> GetAllTaskItemListByUserID(string userId, PageParams paginatedParams);

    }
}

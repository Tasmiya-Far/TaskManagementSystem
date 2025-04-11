using TaskManagementSystem.Core.ViewModel;
using TaskManagementSystem.Infrastructure.ViewModel;

namespace TaskManagementSystem.Infrastructure.Services.Interfaces
{
    public interface ITaskItemService
    {
        Task<TaskItemViewModel> GetTaskDetails(string id);
        TaskItemViewModel AddTaskDetails(TaskItemViewModel taskItem);
        Task<TaskItemViewModel> UpdateTaskDetails(TaskItemViewModel taskItem);
        Task<bool> DeleteTaskDetails(string id);
        Task<PagedResult<DisplayTaskItemViewModel>> GetAllTaskItemDetailsByUserID(string userId, PageParams pageParams);

    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Core.Dbcontext;
using TaskManagementSystem.Core.Intefaces;
using TaskManagementSystem.Core.Models;

namespace TaskManagementSystem.Infrastructure.Repositories
{
    public class TaskItemRepository : GenericRepository<TaskItem>, ITaskItemRepository
    {

        private readonly ILogger<UserRepository> _logger;
        public TaskItemRepository(AppDbContext context, ILogger<UserRepository> logger) : base(context)
        {
            _logger = logger;
        }

        public async Task<List<TaskItem>> GetAllTaskItemListByUserID(string id)
        {
            try
            {
                var taskItemList = new List<TaskItem>();
                taskItemList = await _context.TaskItems.Where(x => x.UserId == new Guid(id) ).ToListAsync();
                return taskItemList;
            }
            catch (Exception ex)
            {
                _logger.LogError("TaskItemRepository/GetAllTaskItemListByUserID" +ex.Message);
                return null;
            }
        }

        public async Task<TaskItem> GetTaskItemByID(string id)
        {
            try
            {
                var user = new TaskItem();
                user = await _context.TaskItems.Where(x => x.Id ==  new Guid(id)).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("TaskItemRepository/GetTaskItemByID" + ex.Message);
                return null;
            }
        }

        public async Task< TaskItem> GetTaskItemListByUserID(string id)
        {
            try
            {
                var user = new TaskItem();
                user = await _context.TaskItems.Where(x => x.Id == new Guid(id))
                    .FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError("TaskItemRepository/GetTaskItemListByUserID" + ex.Message);
                return null;
            }
        }
    }    
}

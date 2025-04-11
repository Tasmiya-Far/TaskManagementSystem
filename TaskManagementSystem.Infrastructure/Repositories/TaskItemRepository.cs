using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Core.Dbcontext;
using TaskManagementSystem.Core.Intefaces;
using TaskManagementSystem.Core.Models;
using TaskManagementSystem.Core.ViewModel;
using TaskManagementSystem.Infrastructure.Services;
using TaskManagementSystem.Infrastructure.ViewModel;

namespace TaskManagementSystem.Infrastructure.Repositories
{
    public class TaskItemRepository : GenericRepository<TaskItem>, ITaskItemRepository
    {

        private readonly ILogger<UserRepository> _logger;
        public TaskItemRepository(AppDbContext context, ILogger<UserRepository> logger) : base(context)
        {
            _logger = logger;
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
        

        public async Task<PagedResult<TaskItem>> GetAllTaskItemListByUserID(string userId, PageParams pageParams)
        {
            var query = _context.TaskItems
                .Where(t => t.UserId == new Guid(userId))
                .OrderByDescending(t => t.Id)
                .Select(t => new TaskItem
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    DueDate = t.DueDate
                });

            // Filter based on Completed
            if (pageParams.Completed . HasValue)
            {
                query = query.Where(t => t.IsCompleted == pageParams.Completed);
            }

            // Filter based on Due Date
            if (pageParams.DueDate.HasValue)
            {
                query = query.Where(t => t.DueDate == pageParams.DueDate);
            }

            var totalCount = await query.CountAsync(); // Total count here

            //Apply pagination 
            var items = await query
                .Skip((pageParams.PageNumber - 1) * pageParams.PageSize)
                .Take(pageParams.PageSize)
                .ToListAsync();
            
            return new PagedResult<TaskItem>
            {
                Tasks = items,
                TotalCount = totalCount,
                PageNumber = pageParams.PageNumber,
                PageSize = pageParams.PageSize
            };
        }
    }    
}

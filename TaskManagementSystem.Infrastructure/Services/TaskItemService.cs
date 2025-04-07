using AutoMapper;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Core;
using TaskManagementSystem.Core.Models;
using TaskManagementSystem.Infrastructure.Services.Interfaces;
using TaskManagementSystem.Infrastructure.ViewModel;

namespace TaskManagementSystem.Infrastructure.Services
{
    public class TaskItemService :  ITaskItemService
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly ILogger<TaskItemService> _logger;

        public TaskItemService(IUnitOfWork unitOfWork, ILogger<TaskItemService> logger)
        {
            UnitOfWork = unitOfWork;
            _logger = logger;
        }

        public TaskItemViewModel AddTaskDetails(TaskItemViewModel taskItem)
        {
            try
            {
                //Add new task item
                var config = new MapperConfiguration(cfg =>
                                   cfg.CreateMap<TaskItemViewModel, TaskItem>()
                                   .ForMember(dest => dest.IsCompleted, act => act.MapFrom(src => src.IsCompleted ? true : false))
                                   .ForMember(dest => dest.DueDate, act => act.MapFrom(src => src.DueDate))
                                   );
                var mapper = new Mapper(config);
                var taskitemdtl = mapper.Map<TaskItemViewModel, TaskItem>(taskItem);
                UnitOfWork.TaskItem.Add(taskitemdtl);
                UnitOfWork.Complete();

                _logger.LogDebug("Task Item details added successfully for ID: " + taskItem.Id);
                return taskItem;
            }
            catch (Exception ex)
            {
                _logger.LogError("TaskItemService/AddTaskDetails failed with an exception: " + ex.Message);
                return taskItem;
            }
        }

        public  async Task<TaskItemViewModel> GetTaskDetails(string id)
        {
            var TaskItemVM = new TaskItemViewModel();
            try
            {
                var TaskItem = await UnitOfWork.TaskItem.GetTaskItemByID(id);

                if (TaskItem != null)
                {
                    TaskItemVM.Id = TaskItem.Id;
                    TaskItemVM.Title = TaskItem.Title;
                    TaskItemVM.Description = TaskItem.Description;
                    TaskItemVM.DueDate = TaskItem.DueDate;
                    TaskItemVM.IsCompleted = TaskItem.IsCompleted;
                    TaskItemVM.UserId = TaskItem.UserId;
                }
                return TaskItemVM;
            }
            catch (Exception ex)
            {
                _logger.LogError("TaskItemService/GetTaskDetails failed with an exception : " + ex.Message);
                return TaskItemVM;
            }
        }

        public async Task<List<TaskItemViewModel>> GetAllTaskItemDetailsByUserID(string id)
        {
            var TaskItemVM = new List<TaskItemViewModel>();
            try
            {
                var TaskItems = await UnitOfWork.TaskItem.GetAllTaskItemListByUserID(id);
                    if (TaskItems != null)
                {
                    foreach (var item in TaskItems)
                    {
                        TaskItemViewModel taskitemVM = new();
                        taskitemVM.Id = item.Id;
                        taskitemVM.Title = item.Title;
                        taskitemVM.Description = item.Description;
                        taskitemVM.IsCompleted = item.IsCompleted;
                        taskitemVM.DueDate = item.DueDate;
                        
                        TaskItemVM.Add(taskitemVM);
                    }
                }
                return TaskItemVM;
            }
            catch (Exception ex)
            {
                _logger.LogError("TaskItemService/GetAllTaskItemDetailsByUserID failed with an exception : " + ex.Message);
                return TaskItemVM;
            }
        }

        public async Task< TaskItemViewModel> UpdateTaskDetails(TaskItemViewModel taskItemVM)
        {
            try
            {
                var TaskItemVM = await UnitOfWork.TaskItem.GetTaskItemByID( taskItemVM.Id.ToString());
                var TaskItem = new TaskItem();

                if (TaskItemVM!=null)
                {
                    TaskItemVM.Title = taskItemVM.Title;
                    TaskItemVM.Description = taskItemVM.Description;
                    TaskItemVM.DueDate = taskItemVM.DueDate;
                    TaskItemVM.IsCompleted = taskItemVM.IsCompleted;
                }
                UnitOfWork.TaskItem.Update(TaskItemVM);
                UnitOfWork.Complete();

                _logger.LogDebug("Task Item details updated successfully for ID: " + taskItemVM.Id);
                return taskItemVM;
            }
            catch (Exception ex)
            {
                _logger.LogError("TaskItemService/UpdateTaskDetails failed with an exception: " + ex.Message);
                return taskItemVM;
            }
        }

        public async Task<bool> DeleteTaskDetails(string id)
        {
            try
            {
                var TaskItem = new TaskItem();
                TaskItem = await UnitOfWork.TaskItem.GetTaskItemByID(id);
                if (TaskItem == null) return false;
                else 
                {
                    UnitOfWork.TaskItem.Delete(TaskItem);
                    UnitOfWork.Complete();
                    _logger.LogDebug("Task Item details deleted successfully"); 
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("TaskItemService/DeleteTaskDetails failed with an exception: " + ex.Message);
                return false;
            }
        }
    }
}

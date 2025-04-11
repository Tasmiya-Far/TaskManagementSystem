using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskManagementSystem.Core;
using TaskManagementSystem.Core.Models;
using TaskManagementSystem.Core.ViewModel;
using TaskManagementSystem.Infrastructure.Services.Interfaces;
using TaskManagementSystem.Infrastructure.ViewModel;

namespace TaskManagementSystem.Infrastructure.Services
{
    public class TaskItemService :  ITaskItemService
    {
        public IUnitOfWork UnitOfWork { get; }
        private readonly ILogger<TaskItemService> _logger;
        public IMapper _mapper;

        public TaskItemService(IUnitOfWork unitOfWork, ILogger<TaskItemService> logger, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public TaskItemViewModel AddTaskDetails(TaskItemViewModel taskItem)
        {
            try
            {
                //Add new task item
                var taskitemdtl = _mapper.Map<TaskItemViewModel, TaskItem>(taskItem);
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
            try
            {
                var TaskItem = await UnitOfWork.TaskItem.GetTaskItemByID(id);
                var TaskItemVM = _mapper.Map<TaskItem, TaskItemViewModel>(TaskItem);
                return TaskItemVM;
            }
            catch (Exception ex)
            {
                _logger.LogError("TaskItemService/GetTaskDetails failed with an exception : " + ex.Message);
                return new TaskItemViewModel();
            }
        }
      
        public async Task< TaskItemViewModel> UpdateTaskDetails(TaskItemViewModel taskItemVM)
        {
            try
            {
                var TaskItemVM = await UnitOfWork.TaskItem.GetTaskItemByID( taskItemVM.Id.ToString());
                if (TaskItemVM!=null)
                {
                    TaskItemVM = _mapper.Map<TaskItemViewModel, TaskItem>(taskItemVM);
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
        public async Task<PagedResult<DisplayTaskItemViewModel>> GetAllTaskItemDetailsByUserID(string userId, PageParams pageParams)
        {
            var pageresult= await UnitOfWork.TaskItem.GetAllTaskItemListByUserID(userId, pageParams);
            var viewModelResult = new PagedResult<DisplayTaskItemViewModel>
            {
                Tasks = _mapper.Map<List<DisplayTaskItemViewModel>>(pageresult.Tasks),                
                TotalCount = pageresult.TotalCount,
                PageNumber = pageresult.PageNumber,
                PageSize = pageresult.PageSize
            };
            return viewModelResult;                
        }
    }
}

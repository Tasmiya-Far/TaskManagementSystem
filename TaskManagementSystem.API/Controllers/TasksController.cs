using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementSystem.Infrastructure.ViewModel;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using TaskManagementSystem.Infrastructure.Services.Interfaces;
using AutoMapper;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {

        private readonly ILogger<TasksController> _logger;

        public ITaskItemService _taskItemService;

        public TasksController( ILogger<TasksController> logger, ITaskItemService taskItemService)
        {
            _logger = logger;
            _taskItemService = taskItemService;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();
                var tasks = await _taskItemService.GetAllTaskItemDetailsByUserID(userId);

                // Map to DisplayTaskItemViewModel (without Id)
                var taskDtos = tasks.Select(t => new DisplayTaskItemViewModel
                {
                    Id = t.Id.ToString(),
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted
                }).ToList();

                if (taskDtos.Any())
                {
                    return Ok(taskDtos);
                }
                else
                {
                    return Ok("No Tasks to display");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("TasksController/GetTasks failed with an exception: " + ex.Message);
                return BadRequest();
            }            
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItemViewModel>> GetTask(string id)
        {
            try
            {
                var userId = GetUserId();
                var task = await _taskItemService.GetTaskDetails(id);
                if (task == null) return NotFound();
                
                return Ok(new DisplayTaskItemViewModel
                {
                    Id = task.Id.ToString(),
                    Title = task.Title,
                    Description = task.Description,
                    DueDate = task.DueDate,
                    IsCompleted = task.IsCompleted
                });
            }
            catch (Exception ex)
            {
                _logger.LogError("TasksController/GetTasks failed with an exception: " + ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("CreateTask")]
        public ActionResult<TaskItemViewModel> CreateTask(CreateTaskItemViewModel CreateItemVM)
        {
            var tasktemViewModel = new TaskItemViewModel();
            try
            {

                if (CreateItemVM != null)
                {
                    //Add details to VM
                    tasktemViewModel.Id = Guid.NewGuid();
                    tasktemViewModel.Title = CreateItemVM.Title;
                    tasktemViewModel.Description = CreateItemVM.Description;
                    tasktemViewModel.DueDate = CreateItemVM.DueDate;
                    tasktemViewModel.IsCompleted = CreateItemVM.IsCompleted;
                    tasktemViewModel.UserId = GetUserId();
                    
                    //Add task details 
                    var TaskItem = _taskItemService.AddTaskDetails(tasktemViewModel);
                }
                return Ok("Task Created Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("TasksController/CreateTask failed with an exception: " + ex.Message);
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(string id, CreateTaskItemViewModel UpdateItemVM)
        {
            try
            {
                var userId = GetUserId();
                var taskVM = new TaskItemViewModel();

                taskVM = await _taskItemService.GetTaskDetails(id);

                if (taskVM == null) return NotFound();

                taskVM.Title = UpdateItemVM.Title;
                taskVM.Description = UpdateItemVM.Description;
                taskVM.DueDate = UpdateItemVM.DueDate;
                taskVM.IsCompleted = UpdateItemVM.IsCompleted;


                //Update task details 
                var TaskItemDtl = _taskItemService.UpdateTaskDetails(taskVM);

                return Ok("Task Updated Successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("TasksController/UpdateTask failed with an exception: " + ex.Message);
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            try
            {
                var userId = GetUserId();

                var Gettask = await _taskItemService.GetTaskDetails(id);
                if (Gettask!=null)
                {
                    var task = await _taskItemService.DeleteTaskDetails(id);
                    return Ok("Task Deleted Successfully");
                }                
                else return Ok("Task Not Found"); 
            }
            catch (Exception ex)
            {
                _logger.LogError("TasksController/DeleteTask failed with an exception: " + ex.Message);
                return BadRequest();
            }
        }
    }
}

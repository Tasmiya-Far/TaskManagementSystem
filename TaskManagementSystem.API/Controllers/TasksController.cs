using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementSystem.Infrastructure.ViewModel;
using Microsoft.AspNetCore.Authorization;
using TaskManagementSystem.Infrastructure.Services.Interfaces;
using TaskManagementSystem.Core.ViewModel;
using AutoMapper;
using System.Threading.Tasks;

namespace TaskManagementSystem.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {

        private readonly ILogger<TasksController> _logger;
        public IMapper _mapper;

        public ITaskItemService _taskItemService;

        public TasksController( ILogger<TasksController> logger, ITaskItemService taskItemService, IMapper mapper)
        {
            _logger = logger;
            _taskItemService = taskItemService;
            _mapper = mapper;
        }

        private Guid GetUserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        [HttpGet]
        public async Task<IActionResult> GetTasksByUserId([FromQuery] PageParams queryParams)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                var tasks = await _taskItemService.GetAllTaskItemDetailsByUserID(userId, queryParams);

                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError("TasksController/GetTasksByUserId failed with an exception: " + ex.Message);
                return BadRequest();
            }            
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<DisplayTaskItemViewModel>> GetTask(string id)
        {
            try
            {
                var userId = GetUserId();
                var task = await _taskItemService.GetTaskDetails(id);
                if (task == null) return NotFound();
                var viewModel = _mapper.Map<DisplayTaskItemViewModel>(task);
                return Ok(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError("TasksController/GetTask failed with an exception: " + ex.Message);
                return BadRequest();
            }
        }

        [HttpPost("CreateTask")]
        public ActionResult<TaskItemViewModel> CreateTask(CreateTaskItemViewModel CreateItemVM)
        {
            try
            {

                if (CreateItemVM != null)
                {
                    // Map CreateTaskItemViewModel to TaskItemViewModel
                    var taskItemViewModel = _mapper.Map<TaskItemViewModel>(CreateItemVM);

                    // Enrich unmapped fields
                    taskItemViewModel.Id = Guid.NewGuid();
                    taskItemViewModel.UserId = GetUserId();
                                        
                    //Add task details 
                    var TaskItem = _taskItemService.AddTaskDetails(taskItemViewModel);
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
        public async Task<IActionResult> UpdateTask(string id, CreateTaskItemViewModel UpdateTaskVM)
        {
            try
            {
                var userId = GetUserId();
                var taskVM = new TaskItemViewModel();

                taskVM = await _taskItemService.GetTaskDetails(id);

                if (taskVM == null) return NotFound();


                // Map CreateTaskItemViewModel -> TaskItemViewModel
                var updatedVM = _mapper.Map<TaskItemViewModel>(UpdateTaskVM);
                updatedVM.Title = UpdateTaskVM.Title;
                updatedVM.Description = UpdateTaskVM.Description;
                updatedVM.IsCompleted = UpdateTaskVM.IsCompleted;
                updatedVM.DueDate = UpdateTaskVM.DueDate;

                //Update task details 
                var TaskItemDtl = _taskItemService.UpdateTaskDetails(updatedVM);

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

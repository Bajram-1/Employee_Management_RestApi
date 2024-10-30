using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Employee_Management.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public TasksController(ITaskService taskService, IProjectService projectService, IUserService userService)
        {
            _taskService = taskService;
            _projectService = projectService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskViewModel>> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<TaskViewModel>> CreateTask(TaskCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = await _taskService.CreateTaskAsync(model);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskUpdateViewModel model)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdString, out var userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var user = await _userService.GetUserByIdAsync(userId);

            if (task == null)
            {
                return NotFound();
            }

            if (task.AssignedEmployeeId != user.Id)
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _taskService.UpdateTaskAsync(id, model);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                return NoContent();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            await _taskService.MarkTaskAsCompletedAsync(id);
            return NoContent();
        }

        [HttpGet("{taskId}/assign")]
        public async Task<ActionResult<TaskAssignmentViewModel>> GetAssignTask(int taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null)
            {
                return NotFound();
            }

            var employees = await _projectService.GetEmployeesAssignedToProjectAsync(task.ProjectId);
            var model = new TaskAssignmentViewModel
            {
                TaskId = taskId,
                Employees = employees
            };

            return Ok(model);
        }

        [HttpPost("{taskId}/assign")]
        public async Task<IActionResult> AssignTask(TaskAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var employees = await _projectService.GetEmployeesAssignedToProjectAsync(model.TaskId);
                model.Employees = employees;
                return BadRequest(ModelState);
            }

            await _taskService.AssignTaskToEmployeeAsync(model.TaskId, model.EmployeeId);
            return NoContent();
        }
    }
}
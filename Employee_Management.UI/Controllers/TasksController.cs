using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Employee_Management.BLL.Services;
using Employee_Management.Common;
using Employee_Management.Common.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Employee_Management.UI.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(ITaskService taskService, IProjectService projectService, IUserService userService, ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _projectService = projectService;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return View(tasks);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TaskCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            await _taskService.CreateTaskAsync(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            //var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (!int.TryParse(userIdString, out var userId))
            //{
            //    return BadRequest("Invalid user ID.");
            //}
            //var user = await _userService.GetUserByIdAsync(userId);

            if (task == null)
            {
                return NotFound();
            }
            //if (task.AssignedEmployeeId != user.Id)
            //{
            //    return Forbid();
            //}

            var model = new TaskUpdateViewModel
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                StartDate = task.StartDate,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                IsCompleted = task.IsCompleted
            };

            ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Common.Enums.TaskStatus)));
            ViewBag.PriorityList = new SelectList(Enum.GetValues(typeof(TaskPriority)));
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TaskUpdateViewModel model)
        {
            //var task = await _taskService.GetTaskByIdAsync(id);
            //var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (!int.TryParse(userIdString, out var userId))
            //{
            //    return BadRequest("Invalid user ID.");
            //}
            //var user = await _userService.GetUserByIdAsync(userId);
            //if (task == null)
            //{
            //    return NotFound();
            //}
            //if (task.AssignedEmployeeId != user.Id)
            //{
            //    return Forbid();
            //}
            if (!ModelState.IsValid)
            {
                ViewBag.StatusList = new SelectList(Enum.GetValues(typeof(Common.Enums.TaskStatus)));
                ViewBag.PriorityList = new SelectList(Enum.GetValues(typeof(TaskPriority)));
                return View(model);
            }

            try
            {
                await _taskService.UpdateTaskAsync(id, model);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _taskService.DeleteTaskAsync(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsCompleted(int id)
        {
            await _taskService.MarkTaskAsCompletedAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet("AssignTask")]
        public async Task<IActionResult> AssignTask(int taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);

            if (task == null)
            {
                return NotFound();
            }

            var allEmployees = await _userService.GetAllUsersAsync();
            var assignedEmployeeIds = await _taskService.GetAssignedEmployeeIdsAsync(taskId);
            var availableEmployees = allEmployees.Where(e => !assignedEmployeeIds.Contains(e.Id)).ToList();

            var model = new TaskAssignmentViewModel
            {
                TaskId = taskId,
                Employees = availableEmployees.Select(e => new EmployeeViewModel
                {
                    Id = e.Id,
                    FullName = e.UserName
                }).ToList()
            };

            return View(model);
        }

        [HttpPost("AssignTask")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignTask(TaskAssignmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var employees = await _projectService.GetEmployeesAssignedToProjectAsync(model.TaskId);
                model.Employees = employees.Select(e => new EmployeeViewModel
                {
                    Id = e.Id,
                    FullName = e.FullName
                }).ToList();

                return View(model);
            }

            await _taskService.AssignTaskToEmployeeAsync(model.TaskId, model.EmployeeId);
            return RedirectToAction("Index", "Project", new { id = model.TaskId });
        }
    }
}
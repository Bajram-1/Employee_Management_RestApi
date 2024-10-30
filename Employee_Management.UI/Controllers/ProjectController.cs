using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Employee_Management.UI.Controllers
{
    //[Authorize]
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;

        public ProjectController(IProjectService projectService, ITaskService taskService, IUserService userService)
        {
            _projectService = projectService;
            _taskService = taskService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            var projectViewModels = projects.Select(project => new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status,
                Assignees = project.Assignees
            }).ToList();
            return View(projectViewModels);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var allEmployees = await _userService.GetAllUsersAsync();
            var model = new ProjectCreateViewModel
            {
                AllEmployees = allEmployees.ToList()
            };
            return View(model);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ProjectCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = await _projectService.CreateProjectAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("CreateTask")]
        public async Task<IActionResult> CreateTask(int projectId)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (string.IsNullOrEmpty(userIdString) ||!int.TryParse(userIdString, out var userId))
            //{
            //    TempData["Error"] = "Unable to identify the user. Please log in again.";
            //    return RedirectToAction("Index", "Project");
            //}

            //if (!await _projectService.IsUserAssignedToProject(projectId, userId))
            //{
            //    TempData["Error"] = "You are not assigned to this project.";
            //    return RedirectToAction("Index", "Project");
            //}

            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var model = new TaskCreateViewModel { ProjectId = projectId };
            return View(model);
        }

        [HttpPost("CreateTask")]
        public async Task<IActionResult> CreateTask(TaskCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validate ProjectId
            var project = await _projectService.GetProjectByIdAsync(model.ProjectId);
            if (project == null)
            {
                ModelState.AddModelError("", "Invalid Project ID.");
                return View(model);
            }

            await _taskService.CreateTaskAsync(model);
            return RedirectToAction("Index", "Project", new { id = model.ProjectId });
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            var projectEditViewModel = new ProjectUpdateViewModel
            {
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
            };

            return View(projectEditViewModel);
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit(int id, ProjectUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _projectService.UpdateProjectAsync(id, model);
            if (!success)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again.");
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var projectEntity = await _projectService.GetProjectByIdAsync(id);
            if (projectEntity == null)
            {
                return NotFound();
            }

            var projectDto = new ProjectViewModel
            {
                Id = projectEntity.Id,
                Name = projectEntity.Name,
                Description = projectEntity.Description,
                StartDate = projectEntity.StartDate,
                EndDate = projectEntity.EndDate
            };

            return View(projectDto);
        }

        [HttpPost("DeleteProject")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (await _projectService.HasOpenTasksAsync(id))
            {
                TempData["Error"] = "Cannot delete project. There are open tasks associated with this project.";
                return RedirectToAction("Index");
            }

            bool success = await _projectService.DeleteProjectAsync(id);
            if (!success)
            {
                TempData["Error"] = "Cannot delete project. There are open tasks associated with this project.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> AddEmployee(int projectId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);
            var users = await _userService.GetAllUsersAsync();
            if (project == null || users == null)
            {
                return NotFound();
            }

            var employeeViewModels = users.Select(u => new EmployeeViewModel
            {
                Id = u.Id,
                FullName = u.UserName
            }).ToList();

            var model = new AddEmployeeViewModel
            {
                ProjectId = project.Id,
                Employees = employeeViewModels
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(int projectId, List<int> assigneeIds)
        {
            if (assigneeIds == null || assigneeIds.Count == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select at least one employee.");
                var allUsers = await _userService.GetAllUsersAsync();
                var allEmployees = allUsers.Select(u => new EmployeeViewModel
                {
                    Id = u.Id,
                    FullName = u.UserName
                }).ToList();

                var addEmployeeModel = new AddEmployeeViewModel
                {
                    ProjectId = projectId,
                    Employees = allEmployees
                };
                return View("AddEmployee", addEmployeeModel);
            }

            foreach (var userId in assigneeIds)
            {
                await _projectService.AddEmployeeToProjectAsync(projectId, userId);
            }

            var projects = await _projectService.GetAllProjectsAsync();
            return RedirectToAction("Index", projects);
        }

        [HttpGet]
        public async Task<IActionResult> RemoveEmployee(int projectId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            var assignedEmployees = await _projectService.GetAssignedEmployeesAsync(projectId);

            var model = new RemoveEmployeeViewModel
            {
                ProjectId = project.Id,
                ProjectName = project.Name,
                AssignedEmployees = assignedEmployees
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveEmployee(int projectId, int employeeId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);

            if (project == null)
            {
                return NotFound();
            }

            var success = await _projectService.RemoveEmployeeFromProjectAsync(projectId, employeeId);

            if (!success)
            {
                ModelState.AddModelError("", "Failed to remove the employee from the project.");
                var assignedEmployees = await _projectService.GetAssignedEmployeesAsync(projectId);
                var model = new RemoveEmployeeViewModel
                {
                    ProjectId = project.Id,
                    ProjectName = project.Name,
                    AssignedEmployees = assignedEmployees
                };
                return View(model);
            }

            return RedirectToAction("Index");
        }
    }
}
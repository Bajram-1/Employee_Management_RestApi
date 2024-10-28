using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Employee_Management.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return View(projects);
        }

        [HttpGet("Create")]
        public async Task<IActionResult> Create()
        {
            var allEmployees = await _userService.GetAllUsersAsync();
            var model = new ProjectCreateViewModel
            {
                AllEmployees = allEmployees
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

        [HttpGet("CreateTask/{projectId}")]
        public async Task<IActionResult> CreateTask(int projectId)
        {
            var model = new TaskCreateViewModel
            {
                ProjectId = projectId
            };

            return View(model);
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

            bool success = await _projectService.UpdateProjectAsync(id, model);
            if (success == false)
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

        [HttpPost("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var isDeleted = await _projectService.DeleteProjectAsync(id);
            if (!isDeleted)
            {
                return NotFound();
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

            var model = new AddEmployeeViewModel
            {
                ProjectId = project.Id,
                EmployeeSelectList = users.ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(int projectId, int assigneeId)
        {
            var project = await _projectService.GetProjectByIdAsync(projectId);
            if (project == null)
            {
                return NotFound();
            }

            project.AssigneeId = assigneeId;
            await _projectService.UpdateProjectAsync(project);

            return RedirectToAction("Index");
        }

        private async Task<IEnumerable<ProjectViewModel>> GetProjectsViewModel()
        {
            var projects = await _projectService.GetAllProjectsAsync() ?? Enumerable.Empty<ProjectViewModel>();
            return projects.Select(project => new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = DateTime.Now,
                EndDate = project.EndDate,
                Employees = project.Employees != null
                            ? project.Employees.Select(e => new EmployeeViewModel
                            {
                                Id = e.Id,
                                FullName = e.FullName
                            }).ToList()
                            : new List<EmployeeViewModel>(),
                AllEmployees = new List<EmployeeViewModel>()
            });
        }
    }
}

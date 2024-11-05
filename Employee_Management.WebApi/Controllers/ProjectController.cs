using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly ITaskService _taskService;
    private readonly IUserService _userService;
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(IProjectService projectService, ITaskService taskService, IUserService userService, ILogger<ProjectController> logger)
    {
        _projectService = projectService;
        _taskService = taskService;
        _userService = userService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProjectViewModel>>> GetProjects()
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

        return Ok(projectViewModels);
    }

    [HttpPost]
    public async Task<ActionResult<ProjectViewModel>> CreateProject(ProjectCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var project = await _projectService.CreateProjectAsync(model);
        var projectViewModel = new ProjectViewModel
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status,
            Assignees = project.ProjectAssignees.Select(pa => pa.User.UserName).ToList()
        };

        return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, projectViewModel);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectViewModel>> GetProjectById(int id)
    {
        var project = await _projectService.GetProjectByIdAsync(id);
        if (project == null)
        {
            return NotFound();
        }

        var projectViewModel = new ProjectViewModel
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Status = project.Status,
            Assignees = project.Assignees
        };

        return Ok(projectViewModel);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, ProjectUpdateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _projectService.UpdateProjectAsync(id, model);
        if (!success)
        {
            return StatusCode(500, "Internal server error while updating project.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        if (await _projectService.HasOpenTasksAsync(id))
        {
            return BadRequest("Cannot delete project. There are open tasks associated with this project.");
        }

        var success = await _projectService.DeleteProjectAsync(id);
        if (!success)
        {
            return StatusCode(500, "Project with that id does not exist in database.");
        }

        return NoContent();
    }

    [HttpPost("createTask")]
    public async Task<IActionResult> CreateTask([FromBody] TaskCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var taskViewModel = await _taskService.CreateTaskAsync(model);
            return Ok(taskViewModel);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "An error occurred while creating the task.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the task. Please try again later.");
        }
    }

    [HttpGet("{projectId}/employees")]
    public async Task<ActionResult<IEnumerable<EmployeeViewModel>>> GetProjectEmployees(int projectId)
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

        return Ok(employeeViewModels);
    }

    [HttpPost("{projectId}/employees")]
    public async Task<IActionResult> AddEmployeesToProject(int projectId, List<int> assigneeIds)
    {
        if (assigneeIds == null || assigneeIds.Count == 0)
        {
            return BadRequest("Please select at least one employee.");
        }

        foreach (var userId in assigneeIds)
        {
            await _projectService.AddEmployeeToProjectAsync(projectId, userId);
        }

        return NoContent();
    }

    [HttpDelete("{projectId}/employees/{employeeId}")]
    public async Task<IActionResult> RemoveEmployeeFromProject(int projectId, int employeeId)
    {
        var success = await _projectService.RemoveEmployeeFromProjectAsync(projectId, employeeId);
        if (!success)
        {
            return StatusCode(500, "Failed to remove the employee from the project.");
        }

        return NoContent();
    }
}
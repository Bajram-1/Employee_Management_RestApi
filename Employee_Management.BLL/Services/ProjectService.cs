using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Employee_Management.DAL;
using Employee_Management.DAL.Entities;
using Employee_Management.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
        private readonly IProjectRepository _projectRepository;

        public ProjectService(ApplicationDbContext context, IUserService userService, IProjectRepository projectRepository)
        {
            _context = context;
            _userService = userService;
            _projectRepository = projectRepository;
        }

        public async Task<List<EmployeeViewModel>> GetEmployeesAssignedToProjectAsync(int projectId)
        {
            return await _context.ProjectAssignees
                                 .Where(pa => pa.ProjectId == projectId)
                                 .Include(pa => pa.User)
                                 .Select(pa => new EmployeeViewModel
                                 {
                                     Id = pa.UserId,
                                     FullName = pa.User.UserName
                                 }).ToListAsync();
        }

        public async Task<IEnumerable<ProjectViewModel>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects
                .Include(p => p.ProjectAssignees)
                .ThenInclude(pa => pa.User)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.StartDate,
                    p.EndDate,
                    p.Status,
                    Assignees = p.ProjectAssignees.Select(pa => pa.User.UserName).ToList()
                }).ToListAsync();

            var projectViewModels = projects.Select(p => new ProjectViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status,
                Assignees = p.Assignees
            }).ToList();

            return projectViewModels;
        }

        public async Task<bool> HasOpenTasksAsync(int projectId)
        {
            return await _context.Tasks
                                 .AnyAsync(t => t.ProjectId == projectId && t.Status != Common.Enums.TaskStatus.Completed);
        }

        public async Task<bool> IsUserAssignedToProject(int projectId, int userId)
        {
            return await _context.ProjectAssignees
                                 .AnyAsync(pa => pa.ProjectId == projectId && pa.UserId == userId);
        }


        public async Task<bool> UpdateProjectAsync(int id, ProjectUpdateViewModel model)
        {
            var project = await _context.Projects.Include(p => p.ProjectAssignees).FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
            {
                return false;
            }

            project.Name = model.Name;
            project.Description = model.Description;
            project.StartDate = model.StartDate;
            project.EndDate = model.EndDate;

            project.ProjectAssignees.Clear();

            foreach (var assigneeId in model.AssigneeIds)
            {
                if (await _context.Users.AnyAsync(u => u.Id == assigneeId))
                {
                    project.ProjectAssignees.Add(new ProjectAssignee { UserId = assigneeId });
                }
                else
                {
                    return false;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task AddEmployeeToProjectAsync(int projectId, int userId)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectAssignees)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project != null)
            {
                var existingAssignment = project.ProjectAssignees
                    .Any(pa => pa.ProjectId == projectId && pa.UserId == userId);

                if (!existingAssignment)
                {
                    project.ProjectAssignees.Add(new ProjectAssignee { ProjectId = projectId, UserId = userId });
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task<ProjectViewModel> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectAssignees)
                    .ThenInclude(pa => pa.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null) return null;

            return new ProjectViewModel
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status,
                AssigneeIds = project.ProjectAssignees.Select(pa => pa.User.Id).ToList(),
                Assignees = project.ProjectAssignees.Select(pa => pa.User.UserName).ToList()
            };
        }
        
        public async Task<List<EmployeeViewModel>> GetAssignedEmployeesAsync(int projectId)
        {
            var assignedEmployees = await _context.ProjectAssignees
                .Where(pa => pa.ProjectId == projectId)
                .Select(pa => new EmployeeViewModel
                {
                    Id = pa.UserId,
                    FullName = pa.User.UserName
                }).ToListAsync();

            return assignedEmployees;
        }

        public async Task<bool> RemoveEmployeeFromProjectAsync(int projectId, int employeeId)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectAssignees)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
            {
                return false;
            }

            var projectAssignee = project.ProjectAssignees
                .FirstOrDefault(pa => pa.UserId == employeeId);

            if (projectAssignee != null)
            {
                project.ProjectAssignees.Remove(projectAssignee);
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<Project> CreateProjectAsync(ProjectCreateViewModel model)
        {
            var project = new Project
            {
                Name = model.Name,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = model.Status.ToString()
            };

            var assignedUsernames = new List<string>();

            foreach (var userName in model.Assignees)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user != null)
                {
                    project.ProjectAssignees.Add(new ProjectAssignee { ProjectId = project.Id, UserId = user.Id });
                    assignedUsernames.Add(user.UserName);
                }
            }

            string usernamesString = string.Join(", ", assignedUsernames);

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new Project
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status,
            };
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            var project = await _context.Projects
                                        .Include(p => p.Tasks)
                                        .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null || project.Tasks.Any(t => t.Status != Common.Enums.TaskStatus.Completed))
            {
                return false;
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
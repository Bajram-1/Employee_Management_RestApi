using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Employee_Management.Common.Enums;
using Employee_Management.DAL;
using Employee_Management.DAL.Entities;
using Employee_Management.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProjectViewModel>> GetAllProjectsAsync()
        {
            var projects = await _context.Projects
                .Include(p => p.ProjectAssignees)
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
                Status = Enum.TryParse<ProjectStatus>(p.Status, out var status) ? status : ProjectStatus.Active
            });

            return projectViewModels;
        }

        public async Task UpdateProjectAsync(Project project)
        {
            _context.Projects.Update(project);
            await _context.SaveChangesAsync();
        }

        public async Task AddEmployeeToProjectAsync(int projectId, int userId)
        {
            var project = await _context.Projects.Include(p => p.ProjectAssignees).FirstOrDefaultAsync(p => p.Id == projectId);
            var employee = await _context.ProjectAssignees.SingleOrDefaultAsync(u => u.UserId == userId);

            if (project != null && employee != null)
            {
                project.ProjectAssignees.Add(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Project> GetProjectByIdAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return null;

            return new Project
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
            };
        }

        public async Task<List<string>> GetUsernamesByIdsAsync(IEnumerable<string> userIds)
        {
            var ids = userIds.Select(int.Parse).ToList();
            var users = await _context.Users
                .Where(u => ids.Contains(u.Id))
                .Select(u => u.UserName)
                .ToListAsync();
            return users;
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

            foreach (var userName in model.Assignees)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
                if (user != null)
                {
                    project.ProjectAssignees.Add(new ProjectAssignee { ProjectId = project.Id, UserId = user.Id });
                }
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return new Project
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status
            };
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
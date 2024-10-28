using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.IServices
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectViewModel>> GetAllProjectsAsync();
        Task<Project> GetProjectByIdAsync(int id);
        Task<Project> CreateProjectAsync(ProjectCreateViewModel model);
        Task UpdateProjectAsync(Project project);
        Task<bool> DeleteProjectAsync(int id);
        Task AddEmployeeToProjectAsync(int projectId, int employeeId);
        Task<List<string>> GetUsernamesByIdsAsync(IEnumerable<string> userIds);
    }
}
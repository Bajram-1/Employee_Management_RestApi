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
        Task<ProjectViewModel> GetProjectByIdAsync(int id);
        Task<Project> CreateProjectAsync(ProjectCreateViewModel model);
        Task<bool> UpdateProjectAsync(int id, ProjectUpdateViewModel model);
        Task<bool> DeleteProjectAsync(int id);
        Task AddEmployeeToProjectAsync(int projectId, int userId);
        Task<List<EmployeeViewModel>> GetAssignedEmployeesAsync(int projectId);
        Task<bool> RemoveEmployeeFromProjectAsync(int projectId, int employeeId);
        Task<bool> HasOpenTasksAsync(int projectId);
        Task<bool> IsUserAssignedToProject(int projectId, int userId);
        Task<List<EmployeeViewModel>> GetEmployeesAssignedToProjectAsync(int projectId);
    }
}
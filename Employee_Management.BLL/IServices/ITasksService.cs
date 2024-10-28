using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.IServices
{
    public interface ITaskService
    {
        Task<IEnumerable<Employee_Management.BLL.DTO.Tasks>> GetAllTasksAsync();
        Task<Tasks> GetTaskByIdAsync(int id);
        Task<Tasks> CreateTaskAsync(TaskCreateViewModel model);
        Task UpdateTaskAsync(int id, TaskUpdateViewModel model);
        Task DeleteTaskAsync(int id);
    }
}
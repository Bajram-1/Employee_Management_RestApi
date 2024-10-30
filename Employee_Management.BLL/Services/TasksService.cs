using Employee_Management.BLL.DTO;
using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Employee_Management.DAL;
using Employee_Management.DAL.Entities;
using Employee_Management.DAL.IRepositories;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Employee_Management.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;

        public TaskService(ApplicationDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<TaskViewModel> CreateTaskAsync(TaskCreateViewModel model)
        {
            var task = new Entities.Tasks
            {
                Title = model.Title,
                Description = model.Description,
                StartDate = model.StartDate,
                DueDate = model.DueDate,
                Status = model.Status,
                Priority = model.Priority,
                IsCompleted = model.IsCompleted,
                ProjectId = model.ProjectId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var taskViewModel = new TaskViewModel
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

            return taskViewModel;
        }

        public async Task<TaskViewModel> GetTaskByIdAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) return null;

            var taskViewModel = new TaskViewModel
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

            return taskViewModel;
        }

        public async Task<IEnumerable<DTO.Tasks>> GetAllTasksAsync()
        {
            var tasks = await _context.Tasks.ToListAsync();
            return tasks.Select(task => new DTO.Tasks
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                StartDate = task.StartDate,
                DueDate = task.DueDate,
                Status = task.Status,
                Priority = task.Priority,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId
            });
        }

        public async Task UpdateTaskAsync(int id, TaskUpdateViewModel model)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) throw new KeyNotFoundException("Task not found");

            task.Title = model.Title;
            task.Description = model.Description;
            task.StartDate = model.StartDate;
            task.DueDate = model.DueDate;
            task.Status = model.Status;
            task.Priority = model.Priority;
            task.IsCompleted = model.IsCompleted;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) throw new KeyNotFoundException("Task not found");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task MarkTaskAsCompletedAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null) throw new KeyNotFoundException("Task not found");

            task.IsCompleted = true;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task AssignTaskToEmployeeAsync(int taskId, int employeeId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null) throw new KeyNotFoundException("Task not found");

            var user = await _userService.GetUserByIdAsync(employeeId);
            if (user == null) throw new KeyNotFoundException("User not found");

            task.AssignedEmployeeId = employeeId;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }
    }
}
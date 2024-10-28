using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.BLL.IServices;
using Employee_Management.DAL;
using Employee_Management.DAL.Entities;
using Employee_Management.DAL.IRepositories;
using Employee_Management.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.Services
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITaskRepository _taskRepository;

        public TaskService(ApplicationDbContext context, ITaskRepository taskRepository)
        {
            _context = context;
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<Employee_Management.BLL.DTO.Tasks>> GetAllTasksAsync()
        {
            var taskEntities = await _context.Tasks.Include(t => t.Project).ToListAsync();

            var taskDTOs = taskEntities.Select(task => new Employee_Management.BLL.DTO.Tasks
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                ProjectId = task.ProjectId,
                Status = task.Status,
                Priority = task.Priority
                //CreatorId = task.CreatorId,
                //AssigneeId = task.AssigneeId
            }).ToList();

            return taskDTOs;
        }

        public async Task<Tasks> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                return null;
            }

            return new Tasks
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                IsCompleted = task.IsCompleted,
                Status = task.Status,
                Priority = task.Priority
            };
        }

        public async Task<Tasks> CreateTaskAsync(TaskCreateViewModel model)
        {
            var taskEntity = new Tasks
            {
                Title = model.Title,
                Description = model.Description,
                DueDate = model.DueDate,
                ProjectId = model.ProjectId
            };

            _context.Tasks.Add(taskEntity);
            await _context.SaveChangesAsync();

            return taskEntity;
        }

        public async Task UpdateTaskAsync(int id, TaskUpdateViewModel model)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found");
            }

            task.Title = model.Title;
            task.Description = model.Description;
            task.DueDate = model.DueDate;
            task.Status = model.Status;
            task.Priority = model.Priority;
            task.IsCompleted = model.IsCompleted;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTaskAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
            {
                throw new Exception("Task not found.");
            }

            await _taskRepository.DeleteAsync(task);
        }
    }
}
using Employee_Management.Common.Enums;
using Employee_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public Employee_Management.Common.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }

        [Required]
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int? AssignedEmployeeId { get; set; }
        public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
    }
}
using Employee_Management.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class TaskCreateViewModel
    {
        [Required(ErrorMessage = "Title is required.")]
        [MaxLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; }

        [MaxLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Due date is required.")]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Project ID is required.")]
        public int ProjectId { get; set; }

        public bool IsCompleted { get; set; } = false;

        [Required(ErrorMessage = "Status is required.")]
        public Common.Enums.TaskStatus Status { get; set; }

        [Required(ErrorMessage = "Priority is required.")]
        public TaskPriority Priority { get; set; }
    }
}
using Employee_Management.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class TaskUpdateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AssigneeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public int ProjectId { get; set; }
        public bool IsCompleted { get; set; }
        public Common.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
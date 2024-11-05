using Employee_Management.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; }
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        [Required]
        public Common.Enums.TaskStatus Status { get; set; }
        [Required]
        public TaskPriority Priority { get; set; }
        [Required]
        public int ProjectId { get; set; }
        public int AssignedEmployeeId { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}
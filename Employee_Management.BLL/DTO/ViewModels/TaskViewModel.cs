using Employee_Management.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class TaskViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public Common.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public int ProjectId { get; set; }
        public int AssignedEmployeeId { get; set; }
        public IEnumerable<SelectListItem> Projects { get; set; }
    }
}
using Employee_Management.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class TaskCreateViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public int ProjectId { get; set; }
        public Common.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
    }
}
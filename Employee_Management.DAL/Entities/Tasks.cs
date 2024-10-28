using Employee_Management.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.DAL.Entities
{
    public class Tasks
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public Common.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }

        //public int CreatorId { get; set; }
        //public User Creator { get; set; }

        //public int AssigneeId { get; set; }
        //public User Assignee { get; set; }
    }
}
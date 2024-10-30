using Employee_Management.Common.Enums;
using Employee_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public List<int> AssigneeIds { get; set; } = new List<int>();
        public List<string> Assignees { get; set; } = new List<string>();
        public List<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
        public List<EmployeeViewModel> AllEmployees { get; set; } = new List<EmployeeViewModel>();
        public List<ProjectAssigneeViewModel> ProjectAssignees { get; set; } = new List<ProjectAssigneeViewModel>();
    }
}
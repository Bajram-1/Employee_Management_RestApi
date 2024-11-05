using Employee_Management.Common.Enums;
using Employee_Management.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class ProjectViewModel
    {
        [Required(ErrorMessage = "Project ID is required.")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Project Name is required.")]
        [MaxLength(200, ErrorMessage = "Project Name cannot exceed 200 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End Date is required.")]
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public List<int> AssigneeIds { get; set; } = new List<int>();
        public List<string> Assignees { get; set; } = new List<string>();
        public List<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
        public List<EmployeeViewModel> AllEmployees { get; set; } = new List<EmployeeViewModel>();
        public List<ProjectAssigneeViewModel> ProjectAssignees { get; set; } = new List<ProjectAssigneeViewModel>();
    }
}
using Employee_Management.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class ProjectUpdateViewModel
    {
        [Required(ErrorMessage = "Project Name is required.")]
        [MaxLength(200, ErrorMessage = "Project Name cannot exceed 200 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Start Date is required.")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End Date is required.")]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public ProjectStatus Status { get; set; }
        public List<int> AssigneeIds { get; set; } = new List<int>();
        public List<EmployeeViewModel> AllEmployees { get; set; } = new List<EmployeeViewModel>();
    }
}
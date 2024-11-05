using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class TaskAssignmentViewModel
    {
        [Required(ErrorMessage = "Task ID is required.")]
        public int TaskId { get; set; }
        [Required(ErrorMessage = "Employee ID is required.")]
        public int EmployeeId { get; set; }
        [Required(ErrorMessage = "At least one employee must be selected.")]
        public List<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
    }
}
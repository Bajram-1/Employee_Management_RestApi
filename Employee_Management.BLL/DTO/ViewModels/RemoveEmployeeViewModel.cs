using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class RemoveEmployeeViewModel
    {
        [Required(ErrorMessage = "Project ID is required.")]
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "Project Name is required.")]
        [MaxLength(200, ErrorMessage = "Project Name cannot exceed 200 characters.")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "At least one employee must be selected to remove.")]
        public List<EmployeeViewModel> AssignedEmployees { get; set; } = new List<EmployeeViewModel>();
    }
}

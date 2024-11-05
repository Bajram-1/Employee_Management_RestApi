using System.ComponentModel.DataAnnotations;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class AddEmployeeViewModel
    {
        [Required(ErrorMessage = "Project ID is required.")]
        public int ProjectId { get; set; }
        [Required(ErrorMessage = "At least one employee must be selected.")]
        public List<int> AssigneeIds { get; set; } = new List<int>();
        public List<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
    }
}

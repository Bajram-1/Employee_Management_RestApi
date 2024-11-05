using System.ComponentModel.DataAnnotations;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, ErrorMessage = "Full name cannot exceed 100 characters.")]
        public string FullName { get; set; }
    }
}
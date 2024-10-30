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
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public List<EmployeeViewModel> AssignedEmployees { get; set; } = new List<EmployeeViewModel>();
        [Required]
        public int SelectedEmployeeId { get; set; }
    }
}

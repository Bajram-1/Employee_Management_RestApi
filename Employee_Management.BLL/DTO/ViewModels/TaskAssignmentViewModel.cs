using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class TaskAssignmentViewModel
    {
        public int TaskId { get; set; }
        public int EmployeeId { get; set; }
        public List<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
    }
}

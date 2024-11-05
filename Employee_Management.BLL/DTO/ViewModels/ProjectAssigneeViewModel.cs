using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class ProjectAssigneeViewModel
    {
        public int EmployeeId { get; set; }
        public EmployeeViewModel Employee { get; set; }
    }
}

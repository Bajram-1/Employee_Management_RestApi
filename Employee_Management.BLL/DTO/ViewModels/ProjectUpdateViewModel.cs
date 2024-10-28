using Employee_Management.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class ProjectUpdateViewModel
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public ProjectStatus Status { get; set; }
        public List<string> Assignees { get; set; } = new List<string>();
        public List<EmployeeViewModel> AllEmployees = new List<EmployeeViewModel>();
    }
}

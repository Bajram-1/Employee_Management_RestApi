using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class AddEmployeeViewModel
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public List<User> EmployeeSelectList { get; set; } = new List<User>();
    }
}

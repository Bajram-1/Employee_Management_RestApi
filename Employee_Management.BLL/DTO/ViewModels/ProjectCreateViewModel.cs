using Employee_Management.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class ProjectCreateViewModel
    {
        [Required(ErrorMessage = "Project Name is required.")]
        [MaxLength(200, ErrorMessage = "Project Name cannot exceed 200 characters.")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Start Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End Date is required.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public ProjectStatus Status { get; set; }
        public List<User> AllEmployees { get; set; } = new List<User>();
        public List<string> Assignees { get; set; } = new List<string>();
    }
}
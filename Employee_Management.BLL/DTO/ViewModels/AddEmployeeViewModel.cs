namespace Employee_Management.BLL.DTO.ViewModels
{
    public class AddEmployeeViewModel
    {
        public int ProjectId { get; set; }
        public List<int> AssigneeIds { get; set; } = new List<int>();
        public List<EmployeeViewModel> Employees { get; set; } = new List<EmployeeViewModel>();
    }
}

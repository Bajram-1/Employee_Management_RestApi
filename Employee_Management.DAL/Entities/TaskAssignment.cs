using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.DAL.Entities
{
    public class TaskAssignment
    {
        public int TaskId { get; set; }
        public Tasks Task { get; set; }

        public int EmployeeId { get; set; }
        public User Employee { get; set; }
    }
}

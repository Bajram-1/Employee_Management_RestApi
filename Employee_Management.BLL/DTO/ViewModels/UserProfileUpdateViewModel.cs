using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class UserProfileUpdateViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public IFormFile ProfilePicture { get; set; }
    }
}
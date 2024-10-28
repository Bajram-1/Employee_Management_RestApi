using Employee_Management.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO.ViewModels
{
    public class UserUpdateViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "UserName is required.")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        public UserRole Role { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        [NotMapped]
        public IFormFile? ProfilePictureFile { get; set; }
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}

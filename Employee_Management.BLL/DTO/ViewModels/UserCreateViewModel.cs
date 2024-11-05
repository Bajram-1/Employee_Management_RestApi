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
    public class UserCreateViewModel
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public UserRole Role { get; set; }

        [StringLength(50, ErrorMessage = "City name cannot exceed 50 characters.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain only numbers.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only image files with .jpg, .jpeg, or .png extensions are allowed.")]
        public IFormFile? ProfilePictureFile { get; set; }
    }
}
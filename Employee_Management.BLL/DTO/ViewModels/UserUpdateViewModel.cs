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
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; }
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        public string? Password { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        public UserRole Role { get; set; }
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain only numbers.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        [NotMapped]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only .jpg, .jpeg, or .png image files are allowed.")]
        public IFormFile? ProfilePictureFile { get; set; }
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}

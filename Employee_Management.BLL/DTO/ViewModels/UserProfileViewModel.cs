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
    public class UserProfileViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(50, ErrorMessage = "Username cannot exceed 50 characters.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        public string Role { get; set; }
        [MaxLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain only numbers.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }
        [NotMapped]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only .jpg, .jpeg, or .png image files are allowed.")]
        public IFormFile? ProfilePictureFile { get; set; }
    }
}
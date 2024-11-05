using Employee_Management.Common.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.BLL.DTO
{
    public class User : IdentityUser<int>
    {
        [Required(ErrorMessage = "Email is required.")]
        [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public new string Email { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public UserRole Role { get; set; }

        [MaxLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string? City { get; set; }

        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain only numbers.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? PhoneNumber { get; set; }

        public string? ProfilePicture { get; set; }

        [NotMapped]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Only .jpg, .jpeg, or .png image files are allowed.")]
        public IFormFile? ProfilePictureFile { get; set; }

        public ICollection<Tasks> AssignedTasks { get; set; } = new List<Tasks>();
        public ICollection<Tasks> CreatedTasks { get; set; } = new List<Tasks>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
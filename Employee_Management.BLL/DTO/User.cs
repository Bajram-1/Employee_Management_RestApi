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
        [Required]
        [MaxLength(100)]
        public new string Email { get; set; }

        public UserRole Role { get; set; }

        public string? City { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePicture { get; set; }

        [NotMapped]
        public IFormFile? ProfilePictureFile { get; set; }

        public ICollection<Tasks> AssignedTasks { get; set; } = new List<Tasks>();
        public ICollection<Tasks> CreatedTasks { get; set; } = new List<Tasks>();
        public ICollection<Project> Projects { get; set; } = new List<Project>();

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
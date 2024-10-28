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

namespace Employee_Management.DAL.Entities
{
    public class User : IdentityUser<int>
    {
        //[Key]
        //public int Id { get; set; }
        //public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string? StreetAddress { get; set; }
        public string City { get; set; }
        public ProjectStatus Status { get; set; }
        public string? ProfilePicture { get; set; }
        [NotMapped]
        public IFormFile? ProfilePictureFile { get; set; }
        public UserProfile Profile { get; set; }
        //public ICollection<Project> Projects { get; set; }
        public ICollection<ProjectAssignee> ProjectAssignees { get; set; } = new List<ProjectAssignee>();
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
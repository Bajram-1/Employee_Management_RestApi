using Employee_Management.DAL.Entities;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Management.DAL
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<ProjectAssignee> ProjectAssignees { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<TaskAssignment> TaskAssignments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Project>()
                .HasOne(p => p.Assignee)
                .WithMany()
                .HasForeignKey(p => p.AssigneeId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProjectAssignee>()
                .HasKey(pa => new { pa.ProjectId, pa.UserId });

            modelBuilder.Entity<ProjectAssignee>()
                .HasOne(pa => pa.Project)
                .WithMany(p => p.ProjectAssignees)
                .HasForeignKey(pa => pa.ProjectId);

            modelBuilder.Entity<ProjectAssignee>()
                .HasOne(pa => pa.User)
                .WithMany(u => u.ProjectAssignees)
                .HasForeignKey(pa => pa.UserId);

            modelBuilder.Entity<Project>()
                .HasMany(p => p.ProjectAssignees)
                .WithOne(u => u.Project);

            modelBuilder.Entity<Project>()
                .Property(e => e.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(u => u.RowVersion)
                .IsRowVersion();

            modelBuilder.Entity<TaskAssignment>()
            .HasKey(ta => new { ta.TaskId, ta.EmployeeId });

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Task)
                .WithMany(t => t.TaskAssignments)
                .HasForeignKey(ta => ta.TaskId);

            modelBuilder.Entity<TaskAssignment>()
                .HasOne(ta => ta.Employee)
                .WithMany()
                .HasForeignKey(ta => ta.EmployeeId);
        }
    }
}
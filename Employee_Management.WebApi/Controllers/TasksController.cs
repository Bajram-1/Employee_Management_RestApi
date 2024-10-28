using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Employee_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] TaskCreateViewModel model)
        {
            var username = User.Identity.Name;
            var user = await _context.Users.Include(u => u.ProjectAssignees).SingleOrDefaultAsync(u => u.UserName == username);

            if (user == null || !user.ProjectAssignees.Any(p => p.ProjectId == model.ProjectId))
            {
                return BadRequest("User is not part of the project.");
            }

            var task = new DAL.Entities.Tasks
            {
                Title = model.Title,
                Description = model.Description,
                IsCompleted = false,
                //AssigneeId = model.AssigneeId,
                ProjectId = model.ProjectId,
                Status = model.Status,
                Priority = model.Priority
                //CreatorId = user.Id
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Project)
                .SingleOrDefaultAsync(t => t.Id == id);

            if (task == null) return NotFound();

            return Ok(task);
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(int id, [FromBody] TaskUpdateViewModel model)
        {
            var task = await _context.Tasks.SingleOrDefaultAsync(t => t.Id == id);
            if (task == null) return NotFound();

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Only the assigned user can update the task
            //if (task.AssigneeId.ToString() != userId) return Forbid();

            task.Title = model.Title;
            task.Description = model.Description;
            task.DueDate = model.DueDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
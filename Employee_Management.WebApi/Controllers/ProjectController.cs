using Employee_Management.BLL.DTO;
using Employee_Management.BLL.DTO.ViewModels;
using Employee_Management.Common;
using Employee_Management.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employee_Management.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(StaticDetails.Role_Admin)]
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var project = new Employee_Management.DAL.Entities.Project
            {
                Name = model.Name,
                StartDate = DateTime.Now,
                EndDate = model.EndDate,
                Description = model.Description,
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id }, project);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProject(int id)
        {
            var project = await _context.Projects.Include(p => p.Tasks).SingleOrDefaultAsync(p => p.Id == id);
            if (project == null) return NotFound();

            return Ok(project);
        }

        [HttpDelete("{id}")]
        [Authorize(StaticDetails.Role_Admin)]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null) return NotFound();

            if (project.Tasks.Any(t => !t.IsCompleted))
            {
                return BadRequest("Cannot delete a project with open tasks.");
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}

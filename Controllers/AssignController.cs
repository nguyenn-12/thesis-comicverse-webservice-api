using Microsoft.AspNetCore.Mvc;
using thesis_comicverse_webservice_api.Models;
using thesis_comicverse_webservice_api.Repositories;

namespace thesis_comicverse_webservice_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AssignController : ControllerBase
    {

        private readonly ILogger<AssignController> _logger;
        private readonly IAssignment _assignmentRepository;

        public AssignController(ILogger<AssignController> logger, IAssignment assignmentRepository)
        {
            _logger = logger;
            _assignmentRepository = assignmentRepository;
        }

        [HttpGet("list-task")]
        public async Task<IActionResult> GetAllAssignments()
        {
            try
            {
                _logger.LogInformation("Getting all assignments");
                var assignments = await _assignmentRepository.GetAllAssignmentsAsync();
                return Ok(assignments);
            }
            catch
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            try
            {
                var assignment = await _assignmentRepository.DeleteAssignmentAsync(id);
                if (assignment == null)
                {
                    return NotFound();
                }
                return Ok(assignment);
            }
            catch
            {
                throw;
            }
        }
    }
}

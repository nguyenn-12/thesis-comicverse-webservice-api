using Microsoft.EntityFrameworkCore;
using thesis_comicverse_webservice_api.Database;
using thesis_comicverse_webservice_api.Models;

namespace thesis_comicverse_webservice_api.Repositories
{
    public interface IAssignment
    {
        Task<IEnumerable<object>> GetAllAssignmentsAsync();
    }

    public class AssignmentRepository : IAssignment
    {
        private readonly AppDbContext _context;

        private readonly ILogger<AssignmentRepository> _logger;
        public AssignmentRepository(AppDbContext context, ILogger<AssignmentRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<object>> GetAllAssignmentsAsync()
        {
            try
            {
                if (_context.Assign == null)
                    throw new ArgumentNullException(nameof(_context.Assign));
                if (_context.Task == null)
                    throw new ArgumentNullException(nameof(_context.Task));
                if (_context.Users == null)
                    throw new ArgumentNullException(nameof(_context.Users));

                var assignments = await (
                    from assign in _context.Assign
                    join task in _context.Task on assign.TaskID equals task.taskID
                    join user in _context.Users on assign.userId equals user.userId
                    select new
                    {
                        TaskID = task.taskID,
                        AssignedTo = user.firstName + " " + user.lastName,
                        TaskStatus = task.status,
                        Priority = task.priority,
                        Complete = task.progressPercentage,
                        AssignAt = assign.assignAt,
                        Deadline = task.deadline,
                        RemainDay = task.deadline - task.deadline
                    }
                    ).ToListAsync();

                return assignments;
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve assignments: {ex.Message}");
            }
        }

        //public Task<IEnumerable<object>> GetAllAssignmentsAysnc()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
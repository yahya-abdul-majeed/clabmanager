using AutoMapper;
using CLabManager_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary.Models;
using ModelsLibrary.Models.DTO;

namespace CLabManager_API.Controllers
{
    [Route("api/[controller]")]
    public class IssuesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public IssuesController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issue>>> GetIssues()
        {
            if (_db.Issues == null)
                return NotFound();
            var issueList = await _db.Issues.Include(c=>c.Computer).Include(l=>l.Lab).ToListAsync();
            return issueList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Issue>> GetIssue(int id)
        {
            if (_db.Issues == null)
                return NotFound();
            //var issue = await _db.Issues.FindAsync(id);
            var issue = await _db.Issues.Include(c => c.Computer).Include(l => l.Lab).Where(x=>x.IssueId== id).FirstOrDefaultAsync();
            if (issue == null)
                return NotFound();
            return issue;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIssue(int id)
        {
            if (_db.Issues == null)
                return NotFound();
            var issue = await _db.Issues.FindAsync(id);
            if(issue == null)
                return NotFound();
            _db.Issues.Remove(issue);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Issue>> PostIssue(IssueCreationDTO issueDTO)
        {
            if (_db.Issues == null)
                return NotFound();
            var issue = _mapper.Map<Issue>(issueDTO);
            _db.Issues.Add(issue);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetIssue), new { id = issue.IssueId }, _mapper.Map<IssueDTO>(issue));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IssueUpdateDTO>> UpdateIssue(int id, IssueUpdateDTO dto)
        {
            if (id != dto.IssueId)
            {
                return BadRequest();
            }
            var issue = await _db.Issues.FindAsync(id);
            if (issue is null)
            {
                return NotFound();
            }
            issue.Priority = dto.Priority;
            issue.State = dto.State;

            _db.Update(issue);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIssue), new { id = issue.IssueId }, issue);
        }

    }
}

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
        public async Task<ActionResult<IEnumerable<IssueDTO>>> GetIssues()
        {
            if (_db.Issues == null)
                return NotFound();
            var issueList = await _db.Issues.ToListAsync();
            var issueDtoList = _mapper.Map<List<IssueDTO>>(issueList);
            return issueDtoList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IssueDTO>> GetIssue(int id)
        {
            if (_db.Issues == null)
                return NotFound();
            var issue = await _db.Issues.FindAsync(id);
            if (issue == null)
                return NotFound();
            return _mapper.Map<IssueDTO>(issue);
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
    }
}

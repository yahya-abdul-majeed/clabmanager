using AutoMapper;
using CLabManager_API.Data;
using CLabManager_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary.Models;
using ModelsLibrary.Models.DTO;
using System.Drawing.Text;

namespace CLabManager_API.Controllers
{
    [Route("api/[controller]")]
    
    public class IssuesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMailService _mailService;

        
        private readonly IMapper _mapper;
        public IssuesController(ApplicationDbContext db, IMapper mapper, IMailService mailService, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _mapper = mapper;
            _userManager = userManager;
            _mailService = mailService;
        }

        [HttpGet,Authorize]
        public async Task<ActionResult<IEnumerable<Issue>>> GetIssues()
        {
            if (_db.Issues == null)
                return NotFound();
            var issueList = await _db.Issues.Include(c=>c.Computer).Include(l=>l.Lab).ToListAsync();
            return issueList;
        }

        [HttpGet("{id}"),Authorize]
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
        [Authorize(Roles = "Admin")]
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

        [HttpPost,Authorize]
        public async Task<ActionResult<Issue>> PostIssue(IssueCreationDTO issueDTO)
        {
            if (_db.Issues == null)
                return NotFound();
            var issue = _mapper.Map<Issue>(issueDTO);
            if(issue.Title == string.Empty || issue.Description == string.Empty)
                return UnprocessableEntity();
            _db.Issues.Add(issue);
            await _db.SaveChangesAsync();

            var emails = await GetAllAdminEmailsAsync(issue);

            var mailData = new MailData(
                to: emails,
                subject: "A new issue has been created",
                body: issue.Title + Environment.NewLine + issue.Description,
                displayName: "CLabManager"
                );

            await _mailService.SendAsync(mailData, new CancellationToken());


            return CreatedAtAction(nameof(GetIssue), new { id = issue.IssueId }, _mapper.Map<IssueDTO>(issue));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IssueUpdateDTO>> UpdateIssue(int id, IssueUpdateDTO dto)
        {
            if (id != dto.IssueId) {
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

        private async Task<List<string>> GetAllAdminEmailsAsync(Issue issue)
        {
            var emails = new List<string>();
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            foreach(var user in users) {
                emails.Add(user.Email);      
            }

            return emails;

        }
    }
}

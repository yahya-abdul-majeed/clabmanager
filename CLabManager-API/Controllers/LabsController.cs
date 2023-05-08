using AutoMapper;
using CLabManager_API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelsLibrary.Models;
using ModelsLibrary.Models.DTO;

namespace CLabManager_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
    public class LabsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public LabsController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Lab>>> GetLabs()
        {
            if(_db.Labs== null)
            {
                return NotFound();
            }
            var labs = await _db.Labs.ToListAsync();
            foreach(var lab in labs )
            {
                var computers = await _db.Computers.Where(c => c.LabId == lab.LabId).AsNoTracking().ToListAsync();
                var computerDTOs = _mapper.Map<List<ComputerDTO>>(computers);
                lab.ComputerList = computerDTOs;
            }

            return labs;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Lab>> GetLab(int id)
        {
            if (_db.Labs == null)
                return NotFound();
            var lab = await _db.Labs.FindAsync(id);
            if(lab != null)
            {
                var computers = await _db.Computers.Where(c => c.LabId == lab.LabId).AsNoTracking().ToListAsync();
                var computerDTOs = _mapper.Map<List<ComputerDTO>>(computers);
                lab.ComputerList = computerDTOs;
                return lab;
            }
            return NotFound();
        }
        

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DeleteLab(int id)
        {
            if (_db.Labs == null)
                return NotFound();
            var lab = await _db.Labs.FindAsync(id);
            if(lab == null)
            {
                return NotFound();
            }
            _db.Labs.Remove(lab);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Lab>> PostLab(LabCreationDTO LabDTO)
        {
            if(_db.Labs == null)
            {
                return Problem("Labs table does not exist in the database");
            }
            var lab = _mapper.Map<Lab>(LabDTO);
            _db.Labs.Add(lab); // no await here as savechanges must wait for lab creation to return? 
            await _db.SaveChangesAsync();
            return CreatedAtAction("GetLab", new {id = lab.LabId}, lab); 
            
        }
        
    }
}

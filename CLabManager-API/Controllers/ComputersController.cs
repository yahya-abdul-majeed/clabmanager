using AutoMapper;
using CLabManager_API.Data;
using ModelsLibrary.Models;
using ModelsLibrary.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CLabManager_API.Controllers
{
    [Route("api/[controller]")]
    public class ComputersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public ComputersController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComputerDTO>>> GetComputers()
        {
            if(_db.Computers == null)
                return NotFound();
            var computerList = await _db.Computers.ToListAsync();
            var computerDtoList = _mapper.Map<List<ComputerDTO>>(computerList);

            return computerDtoList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComputerDTO>> GetComputer(int id)
        {
            if (_db.Computers == null)
                return NotFound();
            var computer = await _db.Computers.FindAsync(id);
            if(computer != null)
            {
                return _mapper.Map<ComputerDTO>(computer);
            }
            return NotFound();
        }

        [Route("unassigned")]
        [HttpGet]
        public async Task<ActionResult<List<ComputerDTO>>> GetUnassignedComputers()
        {
            if(_db.Computers == null)
            {
                return NotFound();
            }
            var computers = await _db.Computers.Where(c => c.LabId == null).ToListAsync();
            var computerDTOs = _mapper.Map<List<ComputerDTO>>(computers);
            return computerDTOs ;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComputer(int id)
        {
            if (_db.Computers == null)
                return NotFound();
            var computer = await _db.Computers.FindAsync(id);
            if (computer != null)
            {
                _db.Remove(computer);
                await _db.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Computer>> PostComputer(ComputerCreationDTO ComputerDTO)
        {
            if (_db.Computers == null)
                return NotFound();
            var computer = _mapper.Map<Computer>(ComputerDTO);
            _db.Computers.Add(computer);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetComputer", new {id = computer.ComputerId}, _mapper.Map<ComputerDTO>(computer));
        }
    }
}

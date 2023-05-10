using AutoMapper;
using CLabManager_API.Data;
using ModelsLibrary.Models;
using ModelsLibrary.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CLabManager_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles ="Admin,User")]
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
            var computers = await _db.Computers.Where(c => c.IsPositioned == false).ToListAsync();
            var computerDTOs = _mapper.Map<List<ComputerDTO>>(computers);
            return computerDTOs ;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Computer>> PostComputer(ComputerCreationDTO ComputerDTO)
        {
            if (_db.Computers == null)
                return NotFound();
            var computer = _mapper.Map<Computer>(ComputerDTO);
            if (computer.ComputerName == string.Empty)
                return UnprocessableEntity();
            _db.Computers.Add(computer);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetComputer", new {id = computer.ComputerId}, _mapper.Map<ComputerDTO>(computer));
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ComputerDTO>> UpdateComputer(int id,ComputerDTO ComputerDTO)
        {
            if (id != ComputerDTO.ComputerId)
                return BadRequest();
            if (_db.Computers == null)
                return NotFound();
            Computer comp = _mapper.Map<Computer>(ComputerDTO);
            _db.Update(comp);
            await _db.SaveChangesAsync();

            return CreatedAtAction("GetComputer", new { id = comp.ComputerId }, ComputerDTO);
        }
        [HttpPut]
        [Route("ispositioned/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ComputerDTO>> UpdatePositionStatus(int id, PositionUpdateDTO dto)
        {
            if (_db.Computers == null)
                return NotFound();
            Computer comp = await _db.Computers.FindAsync(id);
            if(comp == null)
            {
                return NotFound();
            }
            comp.IsPositioned = dto.IsPositioned;
            comp.PositionOnGrid = dto.PositionOnGrid;
            comp.LabId = dto.LabId;
            _db.Update(comp);
            await _db.SaveChangesAsync();
            return CreatedAtAction("GetComputer", new { id = comp.ComputerId }, _mapper.Map<ComputerDTO>(comp));
        }
    }
}

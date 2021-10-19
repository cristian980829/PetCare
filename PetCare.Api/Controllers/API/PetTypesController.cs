using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetCare.Api.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetCare.Api.Controllers.API
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class PetTypesController : ControllerBase
    {
        private readonly DataContext _context;

        public PetTypesController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PetType>>> GetPetTypes()
        {
            return await _context.PetTypes.OrderBy(x => x.Description).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PetType>> GetPetType(int id)
        {
            PetType PetType = await _context.PetTypes.FindAsync(id);

            if (PetType == null)
            {
                return NotFound();
            }

            return PetType;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPetType(int id, PetType PetType)
        {
            if (id != PetType.Id)
            {
                return BadRequest();
            }

            _context.Entry(PetType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe tipo de mascota.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<PetType>> PostPetType(PetType PetType)
        {
            _context.PetTypes.Add(PetType);

            try
            {
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetPetType", new { id = PetType.Id }, PetType);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    return BadRequest("Ya existe tipo de mascota.");
                }
                else
                {
                    return BadRequest(dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePetType(int id)
        {
            PetType PetType = await _context.PetTypes.FindAsync(id);
            if (PetType == null)
            {
                return NotFound();
            }

            _context.PetTypes.Remove(PetType);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

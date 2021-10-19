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
        public class RacesController : ControllerBase
        {
            private readonly DataContext _context;

            public RacesController(DataContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<IEnumerable<Race>>> GetRaces()
            {
                return await _context.Races.OrderBy(x => x.Description).ToListAsync();
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<Race>> GetBrand(int id)
            {
                Race race = await _context.Races.FindAsync(id);

                if (race == null)
                {
                    return NotFound();
                }

                return race;
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> PutRace(int id, Race race)
            {
                if (id != race.Id)
                {
                    return BadRequest();
                }

                _context.Entry(race).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                    return NoContent();
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        return BadRequest("Ya existe esta raza.");
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
            public async Task<ActionResult<Race>> PostRace(Race race)
            {
                _context.Races.Add(race);

                try
                {
                    await _context.SaveChangesAsync();
                    return CreatedAtAction("GetRace", new { id = race.Id }, race);
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        return BadRequest("Ya existe esta raza.");
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
            public async Task<IActionResult> DeleteRace(int id)
            {
                Race race = await _context.Races.FindAsync(id);
                if (race == null)
                {
                    return NotFound();
                }

                _context.Races.Remove(race);
                await _context.SaveChangesAsync();

                return NoContent();
            }
        }
    }

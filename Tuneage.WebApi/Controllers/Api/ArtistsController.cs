using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Domain.Entities;

namespace Tuneage.WebApi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistRepository _repository;

        public ArtistsController(IArtistRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Artists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artist>>> GetArtists()
        {
            return await _repository.GetAllAlphabetical();
        }

        // GET: api/Artists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Artist>> GetArtist(int id)
        {
            var artist = await _repository.GetById(id);

            if (artist == null)
            {
                return NotFound();
            }

            return artist;
        }

        // PUT: api/Artists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtist(int id, Artist artist)
        {
            if (id != artist.ArtistId)
            {
                return BadRequest();
            }

            _repository.SetModified(artist);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtistExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Artists
        [HttpPost]
        public async Task<ActionResult<Artist>> PostArtist(Artist artist)
        {
            if (!artist.IsPrinciple)
                await _repository.Create(new AliasedArtist()
                    { ArtistId = artist.ArtistId, Name = artist.Name, IsBand = artist.IsBand, PrincipleArtistId = artist.PrincipleArtistId }
                );
            else
            {
                if (artist.IsBand)
                    await _repository.Create(new Band() { ArtistId = artist.ArtistId, Name = artist.Name });
                else
                    await _repository.Create(new SoloArtist() { ArtistId = artist.ArtistId, Name = artist.Name });
            }

            return CreatedAtAction("GetArtist", new { id = artist.ArtistId }, artist);
        }

        // DELETE: api/Artists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Artist>> DeleteArtist(int id)
        {
            var artist = await _repository.GetById(id);
            if (artist == null)
            {
                return NotFound();
            }

            await _repository.Delete(id);

            return artist;
        }

        private bool ArtistExists(int id)
        {
            return _repository.Any(id);
        }
    }
}

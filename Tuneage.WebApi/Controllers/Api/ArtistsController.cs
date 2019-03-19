using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuneage.Data.Constants;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Domain.Entities;
using Tuneage.Domain.Services;

namespace Tuneage.WebApi.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistRepository _repository;
        private readonly IArtistService _service;

        public ArtistsController(IArtistRepository repository, IArtistService service)
        {
            _repository = repository;
            _service = service;
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
        public async Task<IActionResult> PutArtist(int id, Artist modifiedArtist)
        {
            if (id != modifiedArtist.ArtistId)
            {
                return BadRequest();
            }

            try
            {
                var preExistingArtist = await _repository.GetById(id);
                if (preExistingArtist != null)
                {
                    switch (preExistingArtist.GetType().ToString())
                    {
                        case ArtistTypes.SoloArtist:
                            _repository.SetModified(_service.TransformSoloArtistForUpdate((SoloArtist)preExistingArtist, modifiedArtist));
                            break;
                        case ArtistTypes.Band:
                            _repository.SetModified(_service.TransformBandForUpdate((Band)preExistingArtist, modifiedArtist));
                            break;
                        case ArtistTypes.AliasedArtist:
                            _repository.SetModified(_service.TransformAliasForUpdate((AliasedArtist)preExistingArtist, modifiedArtist));
                            break;
                    }

                    await _repository.SaveChangesAsync();
                }
                else
                {
                    throw new Exception(ErrorMessages.ArtistIdForUpdateDoesNotExist);
                }
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
            await _repository.Create(_service.TransformArtistForCreation(artist));

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

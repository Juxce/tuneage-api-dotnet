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
    public class ReleasesController : ControllerBase
    {
        private readonly IReleaseRepository _repository;

        public ReleasesController(IReleaseRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Releases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Release>>> GetReleases()
        {
            return await _repository.GetAllAlphabetical();
        }

        // GET: api/Releases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Release>> GetRelease(int id)
        {
            var release = await _repository.GetById(id);

            if (release == null)
            {
                return NotFound();
            }

            return release;
        }

        // PUT: api/Releases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRelease(int id, Release release)
        {
            if (id != release.ReleaseId)
            {
                return BadRequest();
            }

            _repository.SetModified(release);

            try
            {
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReleaseExists(id))
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

        // POST: api/Releases
        [HttpPost]
        public async Task<ActionResult<Release>> PostRelease(Release release)
        {
            if (release.IsByVariousArtists)
                await _repository.Create(new VariousArtistsRelease
                {
                    ReleaseId = release.ReleaseId, LabelId = release.LabelId, Title = release.Title,
                    YearReleased = release.YearReleased
                });
            else
                await _repository.Create(new SingleArtistRelease
                {
                    ReleaseId = release.ReleaseId, LabelId = release.LabelId, Title = release.Title,
                    YearReleased = release.YearReleased, ArtistId = release.ArtistId
                });

            return CreatedAtAction("GetRelease", new { id = release.ReleaseId }, release);
        }

        // DELETE: api/Releases/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Release>> DeleteRelease(int id)
        {
            var release = await _repository.GetById(id);
            if (release == null)
            {
                return NotFound();
            }

            await _repository.Delete(id);

            return release;
        }

        private bool ReleaseExists(int id)
        {
            return _repository.Any(id);
        }
    }
}

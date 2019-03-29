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
    public class ReleasesController : ControllerBase
    {
        private readonly IReleaseRepository _repository;
        private readonly IReleaseService _service;

        public ReleasesController(IReleaseRepository repository, IReleaseService service)
        {
            _repository = repository;
            _service = service;
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
        public async Task<IActionResult> PutRelease(int id, Release modifiedRelease)
        {
            if (id != modifiedRelease.ReleaseId)
            {
                return BadRequest();
            }

            try
            {
                var preExistingRelease = await _repository.GetById(id);
                if (preExistingRelease != null)
                {
                    switch (preExistingRelease.GetType().ToString())
                    {
                        case ReleaseTypes.SingleArtistRelease:
                            _repository.SetModified(_service.TransformSingleArtistReleaseForUpdate((SingleArtistRelease)preExistingRelease, modifiedRelease));
                            break;
                        case ReleaseTypes.VariousArtistsRelease:
                            _repository.SetModified(_service.TransformVariousArtistsReleaseForUpdate((VariousArtistsRelease)preExistingRelease, modifiedRelease));
                            break;
                    }
                }
                else
                {
                    throw new Exception(ErrorMessages.ReleaseIdForUpdateDoesNotExist);
                }

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
            await _repository.Create(_service.TransformReleaseForCreation(release));

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

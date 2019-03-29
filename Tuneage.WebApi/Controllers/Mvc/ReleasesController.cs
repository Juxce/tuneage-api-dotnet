using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Tuneage.Data.Constants;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Domain.Entities;
using Tuneage.Domain.Services;

namespace Tuneage.WebApi.Controllers.Mvc
{
    public class ReleasesController : Controller
    {
        private readonly ILabelRepository _labelRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IReleaseRepository _releaseRepository;
        private readonly IReleaseService _releaseService;

        public ReleasesController(ILabelRepository labelRepository, IArtistRepository artistRepository, IReleaseRepository releaseRepository, IReleaseService releaseService)
        {
            _labelRepository = labelRepository;
            _artistRepository = artistRepository;
            _releaseRepository = releaseRepository;
            _releaseService = releaseService;
        }

        // GET: Releases
        public async Task<IActionResult> Index()
        {
            return View(await _releaseRepository.GetAllAlphabetical());
        }

        // GET: Releases/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var release = await _releaseRepository.GetById((int)id);
            if (release == null)
            {
                return NotFound();
            }

            return View(release);
        }

        // GET: Releases/Create
        public IActionResult Create()
        {
            ViewData["LabelId"] = new SelectList(_labelRepository.GetAllAlphabetical().Result, "LabelId", "Name");
            ViewData["ArtistId"] = new SelectList(_artistRepository.GetAllAlphabetical().Result, "ArtistId", "Name");
            return View();
        }

        // POST: Releases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReleaseId,Title,YearReleased,ReleasedOn,LabelId,IsByVariousArtists,ArtistId")] Release release)
        {
            if (ModelState.IsValid)
            {
                await _releaseRepository.Create(_releaseService.TransformReleaseForCreation(release));

                return RedirectToAction(nameof(Index));
            }
            ViewData["LabelId"] = new SelectList(_labelRepository.GetAllAlphabetical().Result, "LabelId", "Name", release.LabelId);
            ViewData["ArtistId"] = new SelectList(_artistRepository.GetAllAlphabetical().Result, "ArtistId", "Name", release.ArtistId);
            return View(release);
        }

        // GET: Releases/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var release = await _releaseRepository.GetById((int)id);
            if (release == null)
            {
                return NotFound();
            }

            var artistStack = GetArtistStackForCreateAndEdit(_artistRepository.GetAllAlphabetical().Result);

            ViewData["LabelId"] = new SelectList(_labelRepository.GetAllAlphabetical().Result, "LabelId", "Name", release.LabelId);
            ViewData["ArtistId"] = new SelectList(artistStack.ToList(), "ArtistId", "Name", release.ArtistId);
            return View(release);
        }

        // POST: Releases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ReleaseId,Title,YearReleased,ReleasedOn,LabelId,IsByVariousArtists,ArtistId")] Release modifiedRelease)
        {
            if (id != modifiedRelease.ReleaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var preExistingRelease = await _releaseRepository.GetById(id);
                    if (preExistingRelease != null)
                    {
                        switch (preExistingRelease.GetType().ToString())
                        {
                            case ReleaseTypes.SingleArtistRelease:
                                _releaseRepository.SetModified(
                                    _releaseService.TransformSingleArtistReleaseForUpdate((SingleArtistRelease)preExistingRelease, modifiedRelease)
                                );
                                break;
                            case ReleaseTypes.VariousArtistsRelease:
                                _releaseRepository.SetModified(
                                    _releaseService.TransformVariousArtistsReleaseForUpdate((VariousArtistsRelease)preExistingRelease, modifiedRelease)
                                );
                                break;
                        }
                            
                        await _releaseRepository.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception(ErrorMessages.ReleaseIdForUpdateDoesNotExist);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReleaseExists(modifiedRelease.ReleaseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            var artistStack = GetArtistStackForCreateAndEdit(_artistRepository.GetAllAlphabetical().Result);

            ViewData["LabelId"] = new SelectList(_labelRepository.GetAllAlphabetical().Result, "LabelId", "Name", modifiedRelease.LabelId);
            ViewData["ArtistId"] = new SelectList(artistStack.ToList(), "ArtistId", "Name", modifiedRelease.ArtistId);
            return View(modifiedRelease);
        }

        // GET: Releases/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var release = await _releaseRepository.GetById((int)id);
            if (release == null)
            {
                return NotFound();
            }

            return View(release);
        }

        // POST: Releases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var release = await _releaseRepository.GetById(id);
            await _releaseRepository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ReleaseExists(int id)
        {
            return _releaseRepository.Any(id);
        }

        private List<Artist> GetArtistStackForCreateAndEdit(List<Artist> artists)
        {
            List<Artist> artistListForDisplay = new List<Artist>
            {
                new Artist { ArtistId = 0, Name = DefaultValues.ArtistListDefaultForNoSelection }
            };
            artistListForDisplay.AddRange(artists);

            return artistListForDisplay;
        }
    }
}

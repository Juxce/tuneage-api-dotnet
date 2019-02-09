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

namespace Tuneage.WebApi.Controllers.Mvc
{
    public class ReleasesController : Controller
    {
        private readonly ILabelRepository _labelRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IReleaseRepository _releaseRepository;

        public ReleasesController(ILabelRepository labelRepository, IArtistRepository artistRepository, IReleaseRepository releaseRepository)
        {
            _labelRepository = labelRepository;
            _artistRepository = artistRepository;
            _releaseRepository = releaseRepository;
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
                if (release.IsByVariousArtists)
                    await _releaseRepository.Create(new VariousArtistsRelease
                    {
                        ReleaseId = release.ReleaseId, LabelId = release.LabelId, Title = release.Title,
                        YearReleased = release.YearReleased
                    });
                else
                    await _releaseRepository.Create(new SingleArtistRelease
                    {
                        ReleaseId = release.ReleaseId, LabelId = release.LabelId, Title = release.Title,
                        YearReleased = release.YearReleased, ArtistId = release.ArtistId
                    });

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
        public async Task<IActionResult> Edit(int id, [Bind("ReleaseId,Title,YearReleased,ReleasedOn,LabelId,IsByVariousArtists,ArtistId")] Release release)
        {
            if (id != release.ReleaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //var existingRelease = await _releaseRepository.GetById(id);
                    //if (existingRelease != null)
                    //{
                    //    switch (existingRelease.GetType().ToString())
                    //    {
                    //        case ReleaseTypes.SingleArtistRelease:
                    //            var updatedSingleArtistRelease = new SingleArtistRelease
                    //            {
                    //                ReleaseId = release.ReleaseId, LabelId = release.LabelId, Title = release.Title,
                    //                YearReleased = release.YearReleased, ArtistId = release.ArtistId
                    //            };
                    //            await _releaseRepository.Update(id, updatedSingleArtistRelease);
                    //            break;
                    //        case ReleaseTypes.VariousArtistsRelease:
                    //            var updatedVariousArtistsRelease = new VariousArtistsRelease
                    //            {
                    //                ReleaseId = release.ReleaseId, LabelId = release.LabelId, Title = release.Title,
                    //                YearReleased = release.YearReleased
                    //            };
                    //            await _releaseRepository.Update(id, updatedVariousArtistsRelease);
                    //            break;
                    //    }
                    //}
                    //else
                    //{
                    //    throw new Exception(ErrorMessages.ReleaseIdForUpdateDoesNotExist);
                    //}

                    await _releaseRepository.Update(id, release);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReleaseExists(release.ReleaseId))
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

            ViewData["LabelId"] = new SelectList(_labelRepository.GetAllAlphabetical().Result, "LabelId", "Name", release.LabelId);
            ViewData["ArtistId"] = new SelectList(artistStack.ToList(), "ArtistId", "Name", release.ArtistId);
            return View(release);
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

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuneage.Data.Constants;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Domain.Entities;
using Tuneage.Domain.Services;

namespace Tuneage.WebApi.Controllers.Mvc
{
    public class ArtistsController : Controller
    {
        private readonly IArtistRepository _repository;
        private readonly IArtistService _service;

        public ArtistsController(IArtistRepository repository, IArtistService service)
        {
            _repository = repository;
            _service = service;
        }

        // GET: Artists
        public async Task<IActionResult> Index()
        {
            return View(await _repository.GetAllAlphabetical());
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _repository.GetById((int)id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistId,Name,IsBand,IsPrinciple,PrincipalArtistId")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                await _repository.Create(_service.TransformArtistForCreation(artist));

                return RedirectToAction(nameof(Index));
            }
            return View(artist);
        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _repository.GetById((int)id);
            if (artist == null)
            {
                return NotFound();
            }
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtistId,Name,IsBand,IsPrinciple,PrincipalArtistId")] Artist modifiedArtist)
        {
            if (id != modifiedArtist.ArtistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
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
                    if (!ArtistExists(modifiedArtist.ArtistId))
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
            return View(modifiedArtist);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _repository.GetById((int)id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artist = await _repository.GetById(id);
            await _repository.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
            return _repository.Any(id);
        }
    }
}

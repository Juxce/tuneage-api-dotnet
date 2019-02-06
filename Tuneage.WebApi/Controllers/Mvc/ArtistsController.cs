using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuneage.Data.Constants;
using Tuneage.Data.Repositories.Sql.EfCore;
using Tuneage.Domain.Entities;

namespace Tuneage.WebApi.Controllers.Mvc
{
    public class ArtistsController : Controller
    {
        private readonly IArtistRepository _repository;

        public ArtistsController(IArtistRepository repository)
        {
            _repository = repository;
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
        public async Task<IActionResult> Create([Bind("ArtistId,Name,IsBand,IsPrinciple,PrincipleArtistId")] Artist artist)
        {
            if (ModelState.IsValid)
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
        public async Task<IActionResult> Edit(int id, [Bind("ArtistId,Name,IsBand,IsPrinciple,PrincipleArtistId")] Artist artist)
        {
            if (id != artist.ArtistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingArtist = await _repository.GetById(id);
                    if (existingArtist != null)
                    {
                        switch (artist.GetType().ToString())
                        {
                            case ArtistTypes.SoloArtist:
                                var updatedSoloArtist = new SoloArtist()
                                {
                                    ArtistId = artist.ArtistId, Name = artist.Name, IsBand = artist.IsBand, IsPrinciple = artist.IsPrinciple
                                };
                                await _repository.Update(id, updatedSoloArtist);
                                break;
                            case ArtistTypes.Band:
                                var updatedBand = new Band()
                                {
                                    ArtistId = artist.ArtistId,
                                    Name = artist.Name,
                                    IsBand = artist.IsBand,
                                    IsPrinciple = artist.IsPrinciple
                                };
                                await _repository.Update(id, updatedBand);
                                break;
                            case ArtistTypes.AliasedArtist:
                                var updatedAlias = new Band()
                                {
                                    ArtistId = artist.ArtistId,
                                    Name = artist.Name,
                                    IsBand = artist.IsBand,
                                    IsPrinciple = artist.IsPrinciple
                                };
                                await _repository.Update(id, updatedAlias);
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception(ErrorMessages.ArtistIdForUpdateDoesNotExist);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.ArtistId))
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
            return View(artist);
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

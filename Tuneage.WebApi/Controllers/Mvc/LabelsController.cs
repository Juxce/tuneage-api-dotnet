using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tuneage.Data.Repositories.Sql;
using Tuneage.Domain.Entities;

namespace Tuneage.WebApi.Controllers.Mvc
{
    public class LabelsController : Controller
    {
        //private readonly TuneageDataContext _context;
        private readonly IEfCoreMsSqlRepository<Label> _repository;

        public LabelsController(IEfCoreMsSqlRepository<Label> repository)
        {
            //_context = context;
            _repository = repository;
        }

        // GET: Labels
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Labels.ToListAsync()); // Original scaffold call using context directly
            return View(await _repository.GetAll());
        }

        // GET: Labels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var label = await _context.Labels.FirstOrDefaultAsync(m => m.LabelId == id); // Original scaffold call using context directly
            var label = await _repository.Get((int)id);
            if (label == null)
            {
                return NotFound();
            }

            return View(label);
        }

        // GET: Labels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Labels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LabelId,Name,WebsiteUrl")] Label label)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(label); // Original scaffold call using context directly
                _repository.Add(label);
                //await _context.SaveChangesAsync(); // Original scaffold call using context directly
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(label);
        }

        // GET: Labels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var label = await _context.Labels.FindAsync(id); // Original scaffold call using context directly
            var label = await _repository.Get((int) id);
            if (label == null)
            {
                return NotFound();
            }
            return View(label);
        }

        // POST: Labels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LabelId,Name,WebsiteUrl")] Label label)
        {
            if (id != label.LabelId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(label); // Original scaffold call using context directly
                    _repository.Update(label);
                    //await _context.SaveChangesAsync(); // Original scaffold call using context directly
                    await _repository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LabelExists(label.LabelId))
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
            return View(label);
        }

        // GET: Labels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var label = await _context.Labels.FirstOrDefaultAsync(m => m.LabelId == id); // Original scaffold call using context directly
            var label = await _repository.Get((int)id);
            if (label == null)
            {
                return NotFound();
            }

            return View(label);
        }

        // POST: Labels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var label = await _context.Labels.FindAsync(id); // Original scaffold call using context directly
            var label = await _repository.Get(id);
            //_context.Labels.Remove(label); // Original scaffold call using context directly
            _repository.Remove(label);
            //await _context.SaveChangesAsync(); // Original scaffold call using context directly
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LabelExists(int id)
        {
            //return _context.Labels.Any(e => e.LabelId == id); // Original scaffold call using context directly
            return _repository.Any(id);
        }
    }
}

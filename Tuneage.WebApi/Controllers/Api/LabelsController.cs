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
    public class LabelsController : ControllerBase
    {
        //private readonly TuneageDataContext _context;
        private readonly ILabelRepository _repository;

        public LabelsController(ILabelRepository repository)
        {
            //_context = context;
            _repository = repository;
        }

        // GET: api/Labels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Label>>> GetLabels()
        {
            //return await _context.Labels.ToListAsync(); // Original scaffold call using context directly
            return await _repository.GetAllAlphabetical();
        }

        // GET: api/Labels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Label>> GetLabel(int id)
        {
            //var label = await _context.Labels.FindAsync(id);
            var label = await _repository.GetById(id);

            if (label == null)
            {
                return NotFound();
            }

            return label;
        }

        // PUT: api/Labels/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLabel(int id, Label label)
        {
            if (id != label.LabelId)
            {
                return BadRequest();
            }

            /* The line below represents a change from the standard scaffold line. It merely moves the setting of the
             * entity's state onto the context object itself, which avoids a null reference issue at test runtime,
             * where the mocked context's Entry object is missing. */
            //_context.SetModified(label); //_context.Entry(label).State = EntityState.Modified; // Original modified-scaffold call using context directly
            _repository.SetModified(label);

            try
            {
                //await _context.SaveChangesAsync(); // Original scaffold call using context directly
                await _repository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LabelExists(id))
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

        // POST: api/Labels
        [HttpPost]
        public async Task<ActionResult<Label>> PostLabel(Label label)
        {
            //_context.Labels.Add(label); // Original scaffold call using context directly
            await _repository.Create(label);
            //await _context.SaveChangesAsync(); // Original scaffold call using context directly

            return CreatedAtAction("GetLabel", new { id = label.LabelId }, label);
        }

        // DELETE: api/Labels/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Label>> DeleteLabel(int id)
        {
            //var label = await _context.Labels.FindAsync(id); // Original scaffold call using context directly
            var label = await _repository.GetById(id);
            if (label == null)
            {
                return NotFound();
            }

            //_context.Labels.Remove(label); // Original scaffold call using context directly
            await _repository.Delete(id);
            //await _context.SaveChangesAsync(); // Original scaffold call using context directly

            return label;
        }

        private bool LabelExists(int id)
        {
            //return _context.Labels.Any(e => e.LabelId == id); // Original scaffold call using context directly
            return _repository.Any(id);
        }
    }
}

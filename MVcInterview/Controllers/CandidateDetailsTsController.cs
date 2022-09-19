using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVcInterview.Models.CandidateDetailsT;

namespace MVcInterview.Controllers
{
    public class CandidateDetailsTsController : Controller
    {
        private readonly JeetuContext _context;

        public CandidateDetailsTsController(JeetuContext context)
        {
            _context = context;
        }

        // GET: CandidateDetailsTs
        public async Task<IActionResult> Index()
        {
              return View(await _context.CandidateDetailsTs.ToListAsync());
        }

        // GET: CandidateDetailsTs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CandidateDetailsTs == null)
            {
                return NotFound();
            }

            var candidateDetailsT = await _context.CandidateDetailsTs
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidateDetailsT == null)
            {
                return NotFound();
            }

            return View(candidateDetailsT);
        }

        // GET: CandidateDetailsTs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CandidateDetailsTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateId,CandidateName,CandidateExperience,CandidatePhoneNo,CandidateMailId,CandidateSkills,CreatedDate")] CandidateDetailsT candidateDetailsT)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candidateDetailsT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(candidateDetailsT);
        }

        // GET: CandidateDetailsTs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CandidateDetailsTs == null)
            {
                return NotFound();
            }

            var candidateDetailsT = await _context.CandidateDetailsTs.FindAsync(id);
            if (candidateDetailsT == null)
            {
                return NotFound();
            }
            return View(candidateDetailsT);
        }

        // POST: CandidateDetailsTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidateId,CandidateName,CandidateExperience,CandidatePhoneNo,CandidateMailId,CandidateSkills,CreatedDate")] CandidateDetailsT candidateDetailsT)
        {
            if (id != candidateDetailsT.CandidateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidateDetailsT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateDetailsTExists(candidateDetailsT.CandidateId))
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
            return View(candidateDetailsT);
        }

        // GET: CandidateDetailsTs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CandidateDetailsTs == null)
            {
                return NotFound();
            }

            var candidateDetailsT = await _context.CandidateDetailsTs
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidateDetailsT == null)
            {
                return NotFound();
            }

            return View(candidateDetailsT);
        }

        // POST: CandidateDetailsTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CandidateDetailsTs == null)
            {
                return Problem("Entity set 'JeetuContext.CandidateDetailsTs'  is null.");
            }
            var candidateDetailsT = await _context.CandidateDetailsTs.FindAsync(id);
            if (candidateDetailsT != null)
            {
                _context.CandidateDetailsTs.Remove(candidateDetailsT);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateDetailsTExists(int id)
        {
          return _context.CandidateDetailsTs.Any(e => e.CandidateId == id);
        }
    }
}

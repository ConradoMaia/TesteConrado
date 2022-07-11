using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CadastroDeUsinas.Context;
using CadastroDeUsinas.Models;
using ReflectionIT.Mvc.Paging;

namespace CadastroDeUsinas.Controllers
{
    public class UsinasController : Controller
    {
        private readonly AppDbContext _context;

        public UsinasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Usinas
        public async Task<IActionResult> Index(string filter, int pageindex = 1, string sort = "NomeFornecedor")
        {
            var resultado = _context.Usinas.AsNoTracking()
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                resultado = resultado.Where(p => p.UcDaUsina.ToString().Contains(filter) ||
                                            p.NomeFornecedor.Contains(filter));
            }

            var model = await PagingList.CreateAsync(resultado, 20, pageindex, sort, "NomeFornecedor");
            model.RouteValue = new RouteValueDictionary { { "filter", filter } };

            return View(model);
        }

        // GET: Usinas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usinas == null)
            {
                return NotFound();
            }

            var usina = await _context.Usinas
                .FirstOrDefaultAsync(m => m.UsinaId == id);
            if (usina == null)
            {
                return NotFound();
            }

            return View(usina);
        }

        // GET: Usinas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usinas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsinaId,UcDaUsina,IsAtivo,NomeFornecedor")] Usina usina)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usina);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usina);
        }

        // GET: Usinas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usinas == null)
            {
                return NotFound();
            }

            var usina = await _context.Usinas.FindAsync(id);
            if (usina == null)
            {
                return NotFound();
            }
            return View(usina);
        }

        // POST: Usinas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsinaId,UcDaUsina,IsAtivo,NomeFornecedor")] Usina usina)
        {
            if (id != usina.UsinaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usina);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsinaExists(usina.UsinaId))
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
            return View(usina);
        }

        // GET: Usinas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usinas == null)
            {
                return NotFound();
            }

            var usina = await _context.Usinas
                .FirstOrDefaultAsync(m => m.UsinaId == id);
            if (usina == null)
            {
                return NotFound();
            }

            return View(usina);
        }

        // POST: Usinas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usinas == null)
            {
                return Problem("Entity set 'AppDbContext.Usinas'  is null.");
            }
            var usina = await _context.Usinas.FindAsync(id);
            if (usina != null)
            {
                _context.Usinas.Remove(usina);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsinaExists(int id)
        {
          return _context.Usinas.Any(e => e.UsinaId == id);
        }
    }
}

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
        public async Task<IActionResult> Index(string filter, string filter2 = "Selecione", string filter3 = "Selecione", 
                                                      int maxItensPage = 5, int pageindex = 1, string sort = "UcDaUsina")
        {
            var resultado = _context.Usinas.AsNoTracking()
                                      .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter))
            {
                resultado = resultado.Where(p => p.UcDaUsina.ToString().Contains(filter));
            }
            if (filter2 != "Selecione")
            {
                resultado = resultado.Where(p => p.NomeFornecedor.Contains(filter2));
            }
            if (filter3 != "Selecione")
            {
                resultado = resultado.Where(p => p.IsAtivo.ToString().Contains(filter3));
            }

            var model = await PagingList.CreateAsync(resultado, maxItensPage, pageindex, sort, "UcDaUsina");
            model.RouteValue = new RouteValueDictionary { { "filter", filter }, { "filter2", filter2 }, { "filter3", filter3 } };

            return View(model);
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
            if (_context.Usinas.Any(u => u.NomeFornecedor == usina.NomeFornecedor &&
                                   u.UcDaUsina == usina.UcDaUsina))
            {
                ModelState.AddModelError("UcDaUsina", $"Essa usina já está registrada.");
            }
            if (usina.NomeFornecedor == "Selecione")
            {
                ModelState.AddModelError("NomeFornecedor", $"Adicione um fornecedor.");
            }
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

            if (_context.Usinas.Any(u => u.NomeFornecedor == usina.NomeFornecedor &&
                                   u.UcDaUsina == usina.UcDaUsina))
            {
                ModelState.AddModelError("UcDaUsina", $"Essa usina já está registrada.");
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

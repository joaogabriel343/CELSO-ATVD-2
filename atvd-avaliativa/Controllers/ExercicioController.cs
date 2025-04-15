using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using atvd_avaliativa.Models;
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.AspNetCore.Authorization;




namespace atvd_avaliativa.Controllers
{
    [Authorize(Policy = "AdminOrPersonal")]
    public class ExercicioController : Controller
    {
        private readonly AcademiaContext _context;

        public ExercicioController(AcademiaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var exercicios = await _context.Exercicios.ToListAsync();
            return View(exercicios);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var exercicio = await _context.Exercicios
                .FirstOrDefaultAsync(e => e.ExercicioID == id);

            if (exercicio == null) return NotFound();

            return View(exercicio);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exercicio exercicio)
        {
            _context.Add(exercicio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var exercicio = await _context.Exercicios.FindAsync(id);
            if (exercicio == null)
                return NotFound();

            return View(exercicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExercicioID,Nome,Categoria,Descricao")] Exercicio exercicio)
        {
            
                _context.Update(exercicio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
         
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var exercicio = await _context.Exercicios
                .FirstOrDefaultAsync(e => e.ExercicioID == id);

            if (exercicio == null) return NotFound();

            return View(exercicio);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exercicio = await _context.Exercicios.FindAsync(id);
            _context.Exercicios.Remove(exercicio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExercicioExists(int id)
        {
            return _context.Exercicios.Any(e => e.ExercicioID == id);
        }
    }
}

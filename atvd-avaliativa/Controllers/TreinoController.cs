using System;
using atvd_avaliativa.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace atvd_avaliativa.Controllers
{
    public class TreinoController : Controller
    {
        private readonly AcademiaContext _context;

        public TreinoController(AcademiaContext context)
        {
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> Index(string busca)
        {
            var userEmail = User.Identity.Name;
            var aluno = await _context.Alunos.FirstOrDefaultAsync(a => a.Email == userEmail);
            var isPersonal = User.IsInRole("Personal");

            var treinos = _context.Treinos
                .Include(t => t.Aluno)
                .Include(t => t.Personal)
                .Include(t => t.TreinoExercicios)
                    .ThenInclude(te => te.Exercicio)
                .AsQueryable();

            if (aluno != null)
            {
                treinos = treinos.Where(t => t.AlunoID == aluno.AlunoID);
            }

            if (!isPersonal && !string.IsNullOrEmpty(busca))
            {
                treinos = treinos.Where(t =>
                    t.Personal.Nome.Contains(busca) ||
                    t.Aluno.Nome.Contains(busca) ||
                    t.Data.ToString().Contains(busca));
            }

            ViewData["Busca"] = busca;

            return View(await treinos.ToListAsync());
        }



        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var treino = await _context.Treinos
                .Include(t => t.Aluno)
                .Include(t => t.Personal)
                .Include(t => t.TreinoExercicios)
                .FirstOrDefaultAsync(m => m.TreinoID == id);

            if (treino == null) return NotFound();

            return View(treino);
        }

        [Authorize(Roles = "Personal")]
        public IActionResult Create()
        {
            ViewData["AlunoID"] = new SelectList(_context.Alunos, "AlunoID", "Nome");
            ViewData["PersonalID"] = new SelectList(_context.Personais, "PersonalID", "Nome");
            ViewData["Exercicios"] = new MultiSelectList(_context.Exercicios, "ExercicioID", "Nome");
            return View();
        }

        [Authorize(Roles = "Personal")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TreinoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var treino = new Treino
                {
                    AlunoID = model.AlunoID,
                    PersonalID = model.PersonalID,
                    Data = model.Data,
                    Hora = model.Hora,
                    TreinoExercicios = model.ExerciciosSelecionados.Select(id => new TreinoExercicio
                    {
                        ExercicioID = id
                    }).ToList()
                };

                _context.Treinos.Add(treino);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AlunoID"] = new SelectList(_context.Alunos, "AlunoID", "Nome", model.AlunoID);
            ViewData["PersonalID"] = new SelectList(_context.Personais, "PersonalID", "Nome", model.PersonalID);
            ViewData["Exercicios"] = new MultiSelectList(_context.Exercicios, "ExercicioID", "Nome", model.ExerciciosSelecionados);
            return View(model);
        }

        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var treino = await _context.Treinos.FindAsync(id);
            if (treino == null) return NotFound();

            ViewData["AlunoID"] = new SelectList(_context.Alunos, "AlunoID", "Nome", treino.AlunoID);
            ViewData["PersonalID"] = new SelectList(_context.Personais, "PersonalID", "Nome", treino.PersonalID);
            return View(treino);
        }

        [Authorize(Roles = "Personal")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TreinoID,PersonalID,AlunoID,Data,Hora")] Treino treino)
        {
            ViewData["AlunoID"] = new SelectList(_context.Alunos, "AlunoID", "Nome", treino.AlunoID);
            ViewData["PersonalID"] = new SelectList(_context.Personais, "PersonalID", "Nome", treino.PersonalID);
            _context.Update(treino);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Personal")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var treino = await _context.Treinos
                .Include(t => t.Aluno)
                .Include(t => t.Personal)
                .FirstOrDefaultAsync(m => m.TreinoID == id);

            if (treino == null) return NotFound();

            return View(treino);
        }

        [Authorize(Roles = "Personal")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var treino = await _context.Treinos.FindAsync(id);
            if (treino != null)
            {
                _context.Treinos.Remove(treino);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool TreinoExists(int id)
        {
            return _context.Treinos.Any(e => e.TreinoID == id);
        }
    }
}

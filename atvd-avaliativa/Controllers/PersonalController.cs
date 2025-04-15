using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using atvd_avaliativa.Models;
using Microsoft.AspNetCore.Authorization;

[Authorize(Policy = "AdminOrPersonal")]
public class PersonalController : Controller
{
    private readonly AcademiaContext _context;

    public PersonalController(AcademiaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var personais = await _context.Personais.ToListAsync();
        return View(personais);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var personal = await _context.Personais
            .Include(p => p.Alunos)
            .Include(p => p.Treinos)
            .FirstOrDefaultAsync(m => m.PersonalID == id);

        if (personal == null)
            return NotFound();

        return View(personal);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PersonalID,Nome,Especialidade")] Personal personal)
    {
        if (ModelState.IsValid)
        {
            _context.Add(personal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(personal);
    }


    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var personal = await _context.Personais.FindAsync(id);
        if (personal == null)
            return NotFound();

        return View(personal);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("PersonalID,Nome,Especialidade")] Personal personal)
    {
        if (id != personal.PersonalID)
            return NotFound();

        if (ModelState.IsValid)
        {
            _context.Update(personal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(personal);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var personal = await _context.Personais
            .FirstOrDefaultAsync(m => m.PersonalID == id);

        if (personal == null)
            return NotFound();

        return View(personal);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var personal = await _context.Personais.FindAsync(id);

        bool temAlunos = await _context.Alunos.AnyAsync(a => a.PersonalID == id);

        if (temAlunos)
        {
            TempData["MensagemErro"] = "Existem alunos vinculados a esse personal.";
            return RedirectToAction(nameof(Delete), new { id });
        }

        if (personal != null)
        {
            _context.Personais.Remove(personal);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }

}
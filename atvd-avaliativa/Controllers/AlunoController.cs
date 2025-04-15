using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using atvd_avaliativa.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

[Authorize(Policy = "AdminOrPersonal")]
public class AlunoController : Controller
{
    private readonly AcademiaContext _context;

    public AlunoController(AcademiaContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var alunos = await _context.Alunos.Include(a => a.Personal).ToListAsync();
        var personais = _context.Personais.ToList();
        Console.WriteLine(personais.Count);
        return View(alunos);
        

    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var aluno = await _context.Alunos
            .Include(a => a.Personal)
            .FirstOrDefaultAsync(m => m.AlunoID == id);

        if (aluno == null)
            return NotFound();

        return View(aluno);
    }

    public async Task<IActionResult> Create()
    {
        var personal = _context.Personais.Include(e => e.Alunos);
        ViewBag.Personais = _context.Personais.ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Aluno aluno)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Personais = _context.Personais.ToList();
            await _context.SaveChangesAsync();
        }

        _context.Add(aluno);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var aluno = await _context.Alunos
            .Include(a => a.Personal) 
            .FirstOrDefaultAsync(a => a.AlunoID == id);

        if (aluno == null)
            return NotFound();

        ViewBag.PersonalID = new SelectList(_context.Personais, "PersonalID", "Nome", aluno.PersonalID);
        return View(aluno);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("AlunoID,Nome,DataNascimento,Email,Instagram,Telefone,Observacoes,PersonalID")] Aluno aluno)
    {
        if (id != aluno.AlunoID)
            return NotFound();

            _context.Update(aluno);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
       
        return View(aluno);
    }



    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var aluno = await _context.Alunos
            .Include(a => a.Personal)
            .FirstOrDefaultAsync(m => m.AlunoID == id);

        if (aluno == null)
            return NotFound();

        return View(aluno);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var aluno = await _context.Alunos.FindAsync(id);

        if (aluno == null)
        {
            TempData["MensagemErro"] = "Aluno não encontrado.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
            TempData["MensagemSucesso"] = "Aluno deletado com sucesso.";
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("FK_") == true)
            {
                TempData["MensagemErro"] = "Não é possível deletar este aluno porque ele está vinculado a outro registro.";
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao tentar deletar o aluno.";
            }

            return RedirectToAction(nameof(Delete), new { id });
        }

        return RedirectToAction(nameof(Index));
    }

}

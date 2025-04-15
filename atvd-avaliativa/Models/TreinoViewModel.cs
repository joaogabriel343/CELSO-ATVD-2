using System.ComponentModel.DataAnnotations;
namespace atvd_avaliativa.Models;
public class TreinoViewModel
{
    public int PersonalID { get; set; }
    public int AlunoID { get; set; }
    public DateTime Data { get; set; }
    public TimeSpan Hora { get; set; }

    [Required(ErrorMessage = "Selecione pelo menos 4 exercícios.")]
    [MinLength(4, ErrorMessage = "Selecione no mínimo 4 exercícios.")]
    public List<int> ExerciciosSelecionados { get; set; }
}

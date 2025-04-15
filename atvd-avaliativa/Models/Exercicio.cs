using System.ComponentModel.DataAnnotations;

namespace atvd_avaliativa.Models
{
    public class Exercicio
    {
        public int ExercicioID { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo Categoria é obrigatório.")]
        public string Categoria { get; set; }

        [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
        public string Descricao { get; set; }
        public ICollection<TreinoExercicio> TreinoExercicios { get; set; }

    }

}

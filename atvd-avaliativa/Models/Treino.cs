using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace atvd_avaliativa.Models
{
    public class Treino
    {
        public int TreinoID { get; set; }
        public int PersonalID { get; set; }
        [ForeignKey("PersonalID")]
        public Personal Personal { get; set; }

        public int AlunoID { get; set; }
        [ForeignKey("AlunoID")]
        public Aluno Aluno { get; set; }

        public DateTime Data { get; set; }

       
        public TimeSpan Hora { get; set; }


        public ICollection<Exercicio> Exercicios { get; set; }
        public ICollection<TreinoExercicio> TreinoExercicios { get; set; }

    }

}

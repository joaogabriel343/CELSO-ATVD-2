using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace atvd_avaliativa.Models
{
    public class Aluno
    {
        [Key]
        public int AlunoID { get; set; }
        public string Nome { get; set; }

        [Column(TypeName = "DATE")]
        [DataType(DataType.Date)] 
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)] 
        public DateTime DataNascimento { get; set; }

        public string Email { get; set; }
        public string Instagram { get; set; }
        public string Telefone { get; set; }
        public string Observacoes { get; set; }

        public int? PersonalID { get; set; }
        public Personal? Personal { get; set; }
        public ICollection<Treino> Treinos { get; set; }
    }
}

namespace atvd_avaliativa.Models
{
    public class Personal
    {
        public int PersonalID { get; set; }
        public string Nome { get; set; }
        public string Especialidade { get; set; }

        public ICollection<Aluno> Alunos { get; set; } = new List<Aluno>();
        public ICollection<Treino> Treinos { get; set; } = new List<Treino>();
    }


}

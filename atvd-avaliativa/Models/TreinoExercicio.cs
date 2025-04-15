namespace atvd_avaliativa.Models
{
    public class TreinoExercicio
    {
        public int TreinoID { get; set; }
        public Treino Treino { get; set; }

        public int ExercicioID { get; set; }
        public Exercicio Exercicio { get; set; }

        public int Series { get; set; }
        public int Repeticoes { get; set; }
        public string Observacoes { get; set; }
    }
}

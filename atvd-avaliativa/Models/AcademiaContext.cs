using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using atvd_avaliativa.Models;

namespace atvd_avaliativa.Models
{
    public class AcademiaContext : IdentityDbContext<IdentityUser> 
    {
        public AcademiaContext(DbContextOptions<AcademiaContext> options)
            : base(options)
        {
        }
        public DbSet<Personal> Personais { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Treino> Treinos { get; set; }
        public DbSet<Exercicio> Exercicios { get; set; }
        public DbSet<TreinoExercicio> TreinoExercicios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TreinoExercicio>()
                .HasKey(te => new { te.TreinoID, te.ExercicioID });

            modelBuilder.Entity<TreinoExercicio>()
                .HasOne(te => te.Treino)
                .WithMany(t => t.TreinoExercicios)
                .HasForeignKey(te => te.TreinoID);

            modelBuilder.Entity<TreinoExercicio>()
                .HasOne(te => te.Exercicio)
                .WithMany(e => e.TreinoExercicios)
                .HasForeignKey(te => te.ExercicioID)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace atvd_avaliativa.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityUserIdToAluno : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Alunos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Alunos");
        }
    }
}

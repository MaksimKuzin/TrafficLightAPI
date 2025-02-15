using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseImplements.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sequences",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Start = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Missing = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sequences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Observations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Segments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PossibleNumbers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Masks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SequenceId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Observations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Observations_Sequences_SequenceId",
                        column: x => x.SequenceId,
                        principalTable: "Sequences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Observations_SequenceId",
                table: "Observations",
                column: "SequenceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Observations");

            migrationBuilder.DropTable(
                name: "Sequences");
        }
    }
}

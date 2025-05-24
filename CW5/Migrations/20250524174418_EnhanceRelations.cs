using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CW5.Migrations
{
    /// <inheritdoc />
    public partial class EnhanceRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MedicamentPrescription",
                columns: table => new
                {
                    MedicamentsIdMedicament = table.Column<int>(type: "int", nullable: false),
                    PrescriptionsIdPrescription = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicamentPrescription", x => new { x.MedicamentsIdMedicament, x.PrescriptionsIdPrescription });
                    table.ForeignKey(
                        name: "FK_MedicamentPrescription_Medicaments_MedicamentsIdMedicament",
                        column: x => x.MedicamentsIdMedicament,
                        principalTable: "Medicaments",
                        principalColumn: "IdMedicament",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicamentPrescription_Prescriptions_PrescriptionsIdPrescription",
                        column: x => x.PrescriptionsIdPrescription,
                        principalTable: "Prescriptions",
                        principalColumn: "IdPrescription",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicamentPrescription_PrescriptionsIdPrescription",
                table: "MedicamentPrescription",
                column: "PrescriptionsIdPrescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicamentPrescription");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebsiteFirstDraft.Migrations
{
    /// <inheritdoc />
    public partial class CreateWeightLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateTable(
                name: "Weight_Logs",
                columns: table => new
                {
                    WeightLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weight_Logs", x => x.WeightLogId);
                    table.ForeignKey(
                        name: "FK_Weight_Logs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "User_id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Weight_Logs_UserId",
                table: "Weight_Logs",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "Weight_Logs");
        }
    }
}

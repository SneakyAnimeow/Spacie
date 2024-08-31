using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace App.Data.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class RemoveInteractionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InteractionId",
                table: "TTTs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "InteractionId",
                table: "TTTs",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L);
        }
    }
}

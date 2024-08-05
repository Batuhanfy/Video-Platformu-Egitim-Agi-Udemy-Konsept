using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UdemyEgitimPlatformu.Migrations
{
    /// <inheritdoc />
    public partial class deneme2456 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoContentList",
                table: "Videolar",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoContentList",
                table: "Videolar");
        }
    }
}

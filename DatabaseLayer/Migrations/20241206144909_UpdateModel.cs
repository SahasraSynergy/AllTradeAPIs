using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseLayer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    Catagory = table.Column<string>(type: "text", nullable: false),
                    SubCatagory = table.Column<string>(type: "text", nullable: false),
                    XBRLLink = table.Column<string>(type: "text", nullable: false),
                    PDFLink = table.Column<string>(type: "text", nullable: false),
                    AnnouncementDetails = table.Column<string>(type: "text", nullable: false),
                    AnnounementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AnnounementCreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Announcements");
        }
    }
}

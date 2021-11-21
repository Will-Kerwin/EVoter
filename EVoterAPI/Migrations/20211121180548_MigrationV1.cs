using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EVoterAPI.Migrations
{
    public partial class MigrationV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Election",
                columns: table => new
                {
                    electionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    electionStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    electionEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    electionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalVotes = table.Column<int>(type: "int", nullable: true),
                    winnerID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    modifiedTicks = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Election", x => x.electionID);
                });

            migrationBuilder.CreateTable(
                name: "Nominee",
                columns: table => new
                {
                    electionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    association = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nominee", x => x.electionID);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    nomineeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    electionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    totalVotes = table.Column<int>(type: "int", nullable: false),
                    ranking = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    voteID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    electionID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nomineeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    voterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    modifiedTicks = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.voteID);
                });

            migrationBuilder.CreateTable(
                name: "VoterUsers",
                columns: table => new
                {
                    userID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    contactNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    modifiedTicks = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoterUsers", x => x.userID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Election");

            migrationBuilder.DropTable(
                name: "Nominee");

            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.DropTable(
                name: "VoterUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoccerManager.Infra.Data.Migrations
{
    public partial class InitialDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "PlayerPositions",
                columns: new[] { "Id", "PositionName" },
                values: new object[,]
                {
                    { (short)1, "Goalkeeper" },
                    { (short)2, "Defender" },
                    { (short)3, "Midfielder" },
                    { (short)4, "Attacker" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "TransferListStatus",
                columns: new[] { "Id", "StatusName" },
                values: new object[,]
                {
                    { (short)1, "Open" },
                    { (short)2, "Transferred" },
                    { (short)3, "Canceled" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "PlayerPositions",
                keyColumn: "Id",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "PlayerPositions",
                keyColumn: "Id",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "PlayerPositions",
                keyColumn: "Id",
                keyValue: (short)3);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "PlayerPositions",
                keyColumn: "Id",
                keyValue: (short)4);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "TransferListStatus",
                keyColumn: "Id",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "TransferListStatus",
                keyColumn: "Id",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "TransferListStatus",
                keyColumn: "Id",
                keyValue: (short)3);
        }
    }
}

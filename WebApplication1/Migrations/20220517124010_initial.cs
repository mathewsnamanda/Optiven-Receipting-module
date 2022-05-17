using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(nullable: false),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PlotNo = table.Column<string>(nullable: true),
                    Receiptno = table.Column<string>(nullable: true),
                    client = table.Column<string>(nullable: true),
                    accno = table.Column<string>(nullable: true),
                    item = table.Column<string>(nullable: true),
                    paymentfor = table.Column<string>(nullable: true),
                    project = table.Column<string>(nullable: true),
                    receivedby = table.Column<string>(nullable: true),
                    ReceiverEmail = table.Column<string>(nullable: true),
                    Paymode = table.Column<string>(nullable: true),
                    chequenumber = table.Column<string>(nullable: true),
                    copy = table.Column<string>(nullable: true),
                    bcopy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receipts");
        }
    }
}

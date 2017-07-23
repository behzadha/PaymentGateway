using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PaymentGateway.DataModel.Migrations
{
    public partial class PaymentGateway : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    GatewayName = table.Column<string>(nullable: true),
                    InvoiceNumber = table.Column<long>(nullable: false),
                    PaymentToken = table.Column<string>(nullable: false),
                    ReferenceNumber = table.Column<string>(nullable: true),
                    RequestId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentVerification",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    IsVerified = table.Column<bool>(nullable: false),
                    PaymentId = table.Column<int>(nullable: false),
                    StatusId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentVerification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentVerification_Payment_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentVerification_PaymentId",
                table: "PaymentVerification",
                column: "PaymentId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentVerification");

            migrationBuilder.DropTable(
                name: "Payment");
        }
    }
}
